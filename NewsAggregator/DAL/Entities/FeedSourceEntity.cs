namespace RssFeedAggregator.DAL.Entities
{
    /// <summary>
    /// Source of news - url feed
    /// </summary>
    public sealed class FeedSourceEntity : BaseEntity
    {
        /// <summary>
        /// Feed title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Human readable description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Url for obtaining news
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Url originally provided by user
        /// </summary>
        public string OriginalUrl { get; set; } = string.Empty;

        /// <summary>
        /// Related news
        /// </summary>
        public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();
    }
}
