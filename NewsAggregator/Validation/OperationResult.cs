namespace RssFeedAggregator.Validation
{
    public interface IOperationResult
    {
        IReadOnlyList<IOperationFailure> Failures { get; }

        bool Succeed { get; }
    }

    public class OperationResult : IOperationResult
    {
        public IReadOnlyList<IOperationFailure> Failures { get; set; }

        public bool Succeed => Failures.Count == 0;

        public OperationResult(IReadOnlyList<IOperationFailure> failures)
        {
            Failures = failures == null ? new List<IOperationFailure>() : failures;
        }
    }
}
