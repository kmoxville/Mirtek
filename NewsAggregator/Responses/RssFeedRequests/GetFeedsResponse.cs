namespace RssFeedAggregator.Responses.RssFeedRequests
{
    public sealed class GetFeedsResponse
    {
        public int Total { get; set; }

        public List<Post> Posts { get; set; } = null!;
    }
}
