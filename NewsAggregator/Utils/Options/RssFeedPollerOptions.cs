namespace RssFeedAggregator.Utils.Options
{
    public sealed class RssFeedPollerOptions
    {
        public static string Position = nameof(RssFeedPollerOptions);

        public int Interval { get; set; }
    }
}
