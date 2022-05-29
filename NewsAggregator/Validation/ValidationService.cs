using FluentValidation;

namespace RssFeedAggregator.Validation
{
    public interface IValidationService<TRequest>
        where TRequest : class
    {
        Task<IOperationResult> ValidateRequestAsync(TRequest request);
    }

    public abstract class ValidationService<TRequest> : AbstractValidator<TRequest>, IValidationService<TRequest>
        where TRequest : class
    {
        public async Task<IOperationResult> ValidateRequestAsync(TRequest request)
        {
            var validationResult = await ValidateAsync(request)!;

            if (validationResult.IsValid)
            {
                return new OperationResult(ArraySegment<IOperationFailure>.Empty);
            }

            var errorsQuery = validationResult.Errors.Select(error =>
            {
                return new OperationFailure()
                {
                    PropertyName = error.PropertyName,
                    Code = error.ErrorCode,
                    Description = error.ErrorMessage
                };
            });

            return new OperationResult(errorsQuery.ToArray());
        }
    }
}
