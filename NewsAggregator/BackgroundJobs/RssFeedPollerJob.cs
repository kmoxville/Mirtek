using AutoMapper;
using Quartz;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.DAL.UnitOfWork;
using System.ServiceModel.Syndication;
using RssFeedAggregator.DAL;
using Microsoft.EntityFrameworkCore;

namespace RssFeedAggregator.BackgroundJobs
{
    [DisallowConcurrentExecution()]
    public class RssFeedPollerJob : IJob
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RssFeedPollerJob(IHttpClientFactory clientFactory, 
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var source in _unitOfWork.FeedSources.GetAll().ToList())
            {
                using var httpClient = _clientFactory.CreateClient("ResilientClient");
                using var result = await httpClient.GetStreamAsync(source.Url);
                SyndicationFeed feed = SyndicationFeed.Load(System.Xml.XmlReader.Create(result));

                var itemsMap = feed.Items.ToDictionary(item => PostEntity.GetGuidMD5(item.Summary.Text), item => item);

                // filter items that already stored in db
                var keys = itemsMap.Keys.ToArray();
                _unitOfWork.Posts.GetAll()
                    .Where(x => x.FeedSourceEntityId == source.Id && keys.Contains(x.Guid))
                    .Select(x => x.Guid)
                    .ToList()
                    .ForEach(key => itemsMap.Remove(key));

                // add new items
                var newEntities = itemsMap.Select(item =>
                {
                    var newEntity = _mapper.Map<PostEntity>(item.Value);
                    newEntity.FeedSource = source;

                    return newEntity;
                }).ToArray();

                if (!newEntities.Any())
                    return;

                _unitOfWork.Context.Entry(source).State = EntityState.Unchanged;
                await _unitOfWork.Posts.InsertAsync(newEntities);
                
                await _unitOfWork.Save();
            }
        }
    }
}
