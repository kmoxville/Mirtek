namespace RssFeedAggregator.Validation
{
    [Serializable]
    public class OperationException : Exception
    {
        public OperationException(IOperationResult operationResult) : base() 
        {
            Result = operationResult;
        }

        public IOperationResult Result { get; set; }
    }
}
