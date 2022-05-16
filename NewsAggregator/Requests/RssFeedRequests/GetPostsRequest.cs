namespace RssFeedAggregator.Requests.RssFeedRequests
{
    public sealed class GetPostsRequest
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string? Filter { get; set; } = string.Empty;
    }
}
