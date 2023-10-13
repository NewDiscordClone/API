using MediatR;
using Microsoft.Extensions.Logging;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IAuthorizedUserProvider _userProvider;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IAuthorizedUserProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Executing request: {typeof(TRequest).Name} by {_userProvider.GetUserName()}");

            TResponse response = await next();

            _logger.LogInformation($"Request {typeof(TRequest).Name} completed by {_userProvider.GetUserName()}");

            return response;
        }
    }
}
