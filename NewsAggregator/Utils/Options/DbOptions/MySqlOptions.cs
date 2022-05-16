namespace RssFeedAggregator.Utils.Options
{
    public sealed class MySQLOptions : DatabaseOptions
    {
        public static string Position { get; set; } = "ConnectionStrings:" + nameof(MySQLOptions);

        public override string GetConnectionString()
        {
            return $"server={Server};user={User};password={Password};database={Scheme}";
        }
    }
}
