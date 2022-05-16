using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.DAL.UnitOfWork;
using RssFeedAggregator.Requests.RssFeedRequests;
using RssFeedAggregator.Responses.RssFeedRequests;
using RssFeedAggregator.Utils.Options;
using RssFeedAggregator.Validation;
using RssFeedAggregator.Validation.Requests.RssFeedRequests;
using System.Data;
using System.ServiceModel.Syndication;

namespace RssFeedAggregator.Services.RssFeedService
{
    public class RssFeedService : IRssFeedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegisterRequestValidator _registerRequestValidator;
        private readonly IGetPostsRequestValidator _getPostsRequestValidator;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly GeneralOptions _generalOptions;

        public RssFeedService(IHttpClientFactory clientFactory,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRegisterRequestValidator registerRequestValidator,
            IGetPostsRequestValidator getPostsRequestValidator,
            IOptions<GeneralOptions> generalOptions)
        {
            _unitOfWork = unitOfWork;
            _registerRequestValidator = registerRequestValidator;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _getPostsRequestValidator = getPostsRequestValidator;
            _generalOptions = generalOptions.Value;
        }

        public async Task<GetFeedsResponse> GetPostsAsync(GetPostsRequest request)
        {
            var operationResult = await _getPostsRequestValidator.ValidateRequestAsync(request);

            if (!operationResult.Succeed)
                throw new OperationException(operationResult);

            var query = _unitOfWork.Posts.GetAll();

            if (request.Skip.HasValue)
                query = query.Skip(request.Skip.Value);

            if (request.Take.HasValue)
                query = query.Take(request.Take.Value);
            else
                query = query.Take(_generalOptions.DefaultNumItemsInGetPostsQuery);

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(item => EF.Functions.Like(item.Description, $"%{request.Filter}%"));

            return new GetFeedsResponse()
            {
                Posts = query.Select(x => _mapper.Map<Post>(x)).ToList(),
                Total = query.Count()
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var foundEntity = _unitOfWork.FeedSources.GetAll().FirstOrDefault(x => x.Url == request.Url);

            if (foundEntity != null)
                return;

            var operationResult = await _registerRequestValidator.ValidateRequestAsync(request);

            if (!operationResult.Succeed)
                throw new OperationException(operationResult);

            //ToDo handle errors
            using var httpClient = _clientFactory.CreateClient("ResilientClient");
            using var result = await httpClient.GetStreamAsync(request.Url);
            SyndicationFeed feed = SyndicationFeed.Load(System.Xml.XmlReader.Create(result));

            var newFeedEntity = _mapper.Map<FeedSourceEntity>(feed);
            newFeedEntity.Url = request.Url;
            newFeedEntity.OriginalUrl = request.Url;

            await _unitOfWork.FeedSources.InsertAsync(newFeedEntity);
            await _unitOfWork.Save();
        }
    }
}
