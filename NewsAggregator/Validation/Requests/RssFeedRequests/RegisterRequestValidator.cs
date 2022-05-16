using RssFeedAggregator.Requests.RssFeedRequests;
using FluentValidation;
using Microsoft.Extensions.Options;
using RssFeedAggregator.Utils.Options;
using System.ServiceModel.Syndication;

namespace RssFeedAggregator.Validation.Requests.RssFeedRequests
{
    public interface IRegisterRequestValidator : IValidationService<RegisterRequest>
    {

    }

    public sealed class RegisterRequestValidator : ValidationService<RegisterRequest>, IRegisterRequestValidator
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterRequestValidator(IHttpClientFactory clientFactory, IOptions<GeneralOptions> options)
        {
            _httpClientFactory = clientFactory;

            RuleFor(x => x.Url)
                .MaximumLength(options.Value.MaxUrlLength)
                .WithErrorCode("RFS-100.1");

            RuleFor(x => x.Url)
                .MustAsync(async (url, CancellationToken) =>
                {
                    try
                    {
                        using var httpClient = _httpClientFactory.CreateClient("ResilientClient");
                        var result = await httpClient.GetStreamAsync(url);
                        SyndicationFeed feed = SyndicationFeed.Load(System.Xml.XmlReader.Create(result));
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
