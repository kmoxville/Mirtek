namespace RssFeedAggregator.Responses.RssFeedRequests
{
    public sealed class Post
    {
        public int Id { get; set; }

        /// <summary>
        /// Feed title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Human readable description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Url to post
        /// </summary>
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// Sorce Url for obtaining news
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}
