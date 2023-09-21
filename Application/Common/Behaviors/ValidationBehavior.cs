using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Sparkle.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : notnull
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validator == null)
            {
                return await next();
            }

            ValidationResult result = await _validator.ValidateAsync(request, cancellationToken);

            if (result.IsValid)
                return await next();

            throw new ValidationException(result.Errors);
        }
    }

}
