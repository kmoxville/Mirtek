namespace RssFeedAggregator.Utils.Options
{
    public sealed class HttpClientOptions
    {
        public static string Position { get; set; } = nameof(HttpClientOptions);

        public int RetryCount { get; set; }

        public int FirstRetrySeconds { get; set; }
    }
}
