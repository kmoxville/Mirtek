namespace RssFeedAggregator.Validation
{
    public interface IOperationFailure
    {
        public string PropertyName { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }

    public class OperationFailure : IOperationFailure
    {
        public string PropertyName { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public string Code { get; set; } = String.Empty;
    }
}
