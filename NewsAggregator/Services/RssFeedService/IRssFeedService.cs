using RssFeedAggregator.Requests.RssFeedRequests;
using RssFeedAggregator.Responses.RssFeedRequests;

namespace RssFeedAggregator.Services.RssFeedService
{
    public interface IRssFeedService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<GetFeedsResponse> GetPostsAsync(GetPostsRequest request);
    }
}