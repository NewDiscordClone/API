using Microsoft.AspNetCore.Authorization;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization.Handlers
{
    public class OrRequirementHandler : AuthorizationHandler<OrRequirement>
    {
        private readonly IAuthorizationHandlerProvider _handlerProvider;
        private readonly IAuthorizationHandlerContextFactory _contextFactory;

        public OrRequirementHandler(IAuthorizationHandlerProvider handlerProvider, IAuthorizationHandlerContextFactory contextFactory)
        {
            _handlerProvider = handlerProvider;
            _contextFactory = contextFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OrRequirement requirement)
        {
            AuthorizationHandlerContext requirementContext = _contextFactory.CreateContext(
                new List<IAuthorizationRequirement> { requirement.Firstly, requirement.Secondary },
                context.User, null);

            IEnumerable<IAuthorizationHandler> handlers = await _handlerProvider
                .GetHandlersAsync(requirementContext);

            foreach (IAuthorizationHandler handler in handlers)
            {
                await handler.HandleAsync(requirementContext);
                if (requirementContext.HasSucceeded)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}
