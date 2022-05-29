using System.ServiceModel.Syndication;

namespace RssFeedAggregator.Services.RssFeedDownloader
{
    public sealed class RssFeedDownloaderService : IRssFeedDownloaderService
    {
        private readonly IHttpClientFactory _factory;

        public RssFeedDownloaderService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<SyndicationFeed> GetSyndicationFeed(string Url)
        {
            using var httpClient = _factory.CreateClient(Settings.RESILIENT_CLIENT);
            using var result = await httpClient.GetStreamAsync(Url);

            return SyndicationFeed.Load(System.Xml.XmlReader.Create(result));
        }
    }
}
