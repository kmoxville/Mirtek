using RssFeedAggregator.Requests.RssFeedRequests;
using FluentValidation;
using Microsoft.Extensions.Options;
using RssFeedAggregator.Utils.Options;
using System.ServiceModel.Syndication;
using RssFeedAggregator.Services.RssFeedDownloader;

namespace RssFeedAggregator.Validation.Requests.RssFeedRequests
{
    public interface IRegisterRequestValidator : IValidationService<RegisterRequest>
    {

    }

    public sealed class RegisterRequestValidator : ValidationService<RegisterRequest>, IRegisterRequestValidator
    {
        private readonly IRssFeedDownloaderService _rssFeedDownloaderService;

        public RegisterRequestValidator(IRssFeedDownloaderService rssFeedDownloaderService, IOptions<GeneralOptions> options)
        {
            _rssFeedDownloaderService = rssFeedDownloaderService;

            RuleFor(x => x.Url)
                .MaximumLength(options.Value.MaxUrlLength)
                .WithErrorCode("RFS-100.1");

            RuleFor(x => x.Url)
                .MustAsync(async (url, CancellationToken) =>
                {
                    try
                    {
                        await _rssFeedDownloaderService.GetSyndicationFeed(url);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    return true;
                })
                .WithErrorCode("RFS-100.2")
                .WithMessage("Invalid url");
        }
    }
}
