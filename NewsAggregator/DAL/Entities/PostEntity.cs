using Microsoft.EntityFrameworkCore;

namespace RssFeedAggregator.DAL.Entities
{
    /// <summary>
    /// Item from rss feed
    /// </summary>
    [Index(nameof(Guid))]
    public sealed class PostEntity : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public DateTime PublishedAt { get; set; }

        public string Guid { get; set; } = string.Empty;

        public int FeedSourceEntityId { get; private set; }

        public FeedSourceEntity FeedSource { get; set; } = null!;

        public static string GetGuidMD5(string guid) => MD5Hash.Hash.Content(guid);
    }
}
