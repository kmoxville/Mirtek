namespace RssFeedAggregator.Utils.Options
{
    public abstract class DatabaseOptions : IDatabaseConnectionStringProvider
    {
        public string Server { get; set; } = string.Empty;

        public string Scheme { get; set; } = string.Empty;

        public string User { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public abstract string GetConnectionString();
    }
}
