using Microsoft.AspNetCore.Mvc;
using RssFeedAggregator.Requests.RssFeedRequests;
using RssFeedAggregator.Responses.RssFeedRequests;
using RssFeedAggregator.Services.RssFeedService;
using RssFeedAggregator.Validation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RssFeedAggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class RssFeedController : ControllerBase
    {
        private readonly IRssFeedService _rssFeedService;

        public RssFeedController(IRssFeedService rssFeedService)
        {
            _rssFeedService = rssFeedService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterRssFeed([FromBody] RegisterRequest request)
        {
            try
            {
                await _rssFeedService.RegisterAsync(request);
            }
            catch (OperationException ex)
            {
                return BadRequest(ex.Result);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("posts")]
        public async Task<ActionResult> GetPosts([FromQuery] GetPostsRequest request)
        {
            GetFeedsResponse response;

            try
            {
                response = await _rssFeedService.GetPostsAsync(request);
            }
            catch (OperationException ex)
            {
                return BadRequest(ex.Result);
            }

            return Ok(response);
        }
    }
}
