using System.ServiceModel.Syndication;

namespace RssFeedAggregator.Services.RssFeedDownloader
{
    public interface IRssFeedDownloaderService
    {
        Task<SyndicationFeed> GetSyndicationFeed(string Url);
    }
}