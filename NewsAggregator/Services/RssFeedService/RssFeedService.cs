using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.DAL.UnitOfWork;
using RssFeedAggregator.Requests.RssFeedRequests;
using RssFeedAggregator.Responses.RssFeedRequests;
using RssFeedAggregator.Services.RssFeedDownloader;
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
        private readonly IRssFeedDownloaderService _rssFeedDownloader;
        private readonly IMapper _mapper;
        private readonly GeneralOptions _generalOptions;
        private readonly ILogger<RssFeedService> _logger;

        public RssFeedService(IRssFeedDownloaderService rssFeedDownloader,
            IUnitOfWork unitOfWork,
            ILogger<RssFeedService> logger,
            IMapper mapper,
            IRegisterRequestValidator registerRequestValidator,
            IGetPostsRequestValidator getPostsRequestValidator,
            IOptions<GeneralOptions> generalOptions)
        {
            _unitOfWork = unitOfWork;
            _registerRequestValidator = registerRequestValidator;
            _rssFeedDownloader = rssFeedDownloader;
            _mapper = mapper;
            _getPostsRequestValidator = getPostsRequestValidator;
            _generalOptions = generalOptions.Value;
            _logger = logger;
        }

        public async Task<GetFeedsResponse> GetPostsAsync(GetPostsRequest request)
        {
            var operationResult = await _getPostsRequestValidator.ValidateRequestAsync(request);

            if (!operationResult.Succeed)
                throw new OperationException(operationResult);

            var query = _unitOfWork.Posts.GetAll().AsNoTracking();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(item => EF.Functions.Like(item.Description, $"%{request.Filter}%"));

            int count = query.Count();

            query = query.OrderBy(x => x.Id);

            if (request.Skip.HasValue)
                query = query.Skip(request.Skip.Value);

            if (request.Take.HasValue)
                query = query.Take(request.Take.Value);
            else
                query = query.Take(_generalOptions.DefaultNumItemsInGetPostsQuery);

            return new GetFeedsResponse()
            {
                Posts = query.Select(x => _mapper.Map<Post>(x)).ToList(),
                Total = count
            };
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            FeedSourceEntity? foundEntity = _unitOfWork.FeedSources.GetAll().FirstOrDefault(x => x.Url == request.Url);
            SyndicationFeed feed;

            if (foundEntity != null)
                return;

            var operationResult = await _registerRequestValidator.ValidateRequestAsync(request);

            if (!operationResult.Succeed)
                throw new OperationException(operationResult);

            try
            {
                feed = await _rssFeedDownloader.GetSyndicationFeed(request.Url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return;
            }

            var newFeedEntity = _mapper.Map<FeedSourceEntity>(feed);
            newFeedEntity.Url = request.Url;
            newFeedEntity.OriginalUrl = request.Url;

            await _unitOfWork.FeedSources.InsertAsync(newFeedEntity);
            await _unitOfWork.Save();
        }
    }
}
