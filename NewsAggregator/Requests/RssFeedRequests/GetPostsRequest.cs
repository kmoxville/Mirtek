namespace RssFeedAggregator.Requests.RssFeedRequests
{
    /// <summary>
    /// Request with pagination
    /// </summary>
    public sealed class GetPostsRequest
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string? Filter { get; set; } = string.Empty;
    }
}
