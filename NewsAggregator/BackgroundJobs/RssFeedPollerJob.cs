using AutoMapper;
using Quartz;
using RssFeedAggregator.DAL.Entities;
using RssFeedAggregator.DAL.UnitOfWork;
using System.ServiceModel.Syndication;
using RssFeedAggregator.DAL;
using Microsoft.EntityFrameworkCore;
using RssFeedAggregator.Services.RssFeedDownloader;

namespace RssFeedAggregator.BackgroundJobs
{
    [DisallowConcurrentExecution()]
    public class RssFeedPollerJob : IJob
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RssFeedPollerJob> _logger;
        private readonly IRssFeedDownloaderService _rssFeedDownloader;

        public RssFeedPollerJob(IRssFeedDownloaderService rssFeedDownloader, 
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<RssFeedPollerJob> logger)
        {
            _rssFeedDownloader = rssFeedDownloader;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            foreach (var source in _unitOfWork.FeedSources.GetAll().AsNoTracking().ToList())
            {
                SyndicationFeed feed;

                try
                {
                    feed = await _rssFeedDownloader.GetSyndicationFeed(source.Url);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return;
                }

                var feedEntry = await _unitOfWork.FeedSources.GetAll()
                    .Where(x => x.Id == source.Id)
                    .Include(x => x.Posts)
                    .FirstOrDefaultAsync();

                if (feedEntry == null)
                    return;

                feedEntry.AddPosts(feed.Items.Select(item => _mapper.Map<PostEntity>(item)));
                
                await _unitOfWork.Save();
            }
        }
    }
}
