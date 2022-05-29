using Microsoft.Extensions.Options;
using RssFeedAggregator.Requests.RssFeedRequests;
using RssFeedAggregator.Utils.Options;
using FluentValidation;

namespace RssFeedAggregator.Validation.Requests.RssFeedRequests
{
    public interface IGetPostsRequestValidator : IValidationService<GetPostsRequest>
    {

    }

    public sealed class GetPostsRequestValidator : ValidationService<GetPostsRequest>, IGetPostsRequestValidator
    {
        public GetPostsRequestValidator(IOptions<GeneralOptions> options)
        {
            RuleFor(x => x.Take!.Value)
                .GreaterThan(0).When(x => x.Take.HasValue)
                .WithErrorCode("RFS-103.1")
                .LessThanOrEqualTo(options.Value.MaxNumItemsInGetPostsQuery).When(x => x.Take.HasValue)
                .WithErrorCode("RFS-103.2");

            RuleFor(x => x.Filter)
                .MaximumLength(options.Value.MaxFilterStringLengthQuery)
                .WithErrorCode("RFS-103.3");
        }
    }
}
