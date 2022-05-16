namespace RssFeedAggregator.Utils.Options
{
    public sealed class GeneralOptions
    {
        public static string Position { get; set; } = nameof(GeneralOptions);

        public int MaxUrlLength { get; set; }
        public int MaxDescriptionLength { get; set; }

        public int MaxNumItemsInGetPostsQuery { get; set; }
        public int DefaultNumItemsInGetPostsQuery { get; set; }

        public int MaxFilterStringLengthQuery { get; set; }
    }
}
