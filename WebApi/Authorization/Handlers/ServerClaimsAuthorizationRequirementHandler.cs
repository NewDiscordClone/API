using Microsoft.AspNetCore.Authorization;
using Sparkle.Application.Common.Interfaces;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization.Handlers
{
    public class ServerClaimsAuthorizationRequirementHandler :
        AuthorizationHandler<ServerClaimsAuthorizationRequirement>
    {
        private readonly IAuthorizedUserProvider _userProvider;

        public ServerClaimsAuthorizationRequirementHandler(IAuthorizedUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ServerClaimsAuthorizationRequirement requirement)
        {
            if (await _userProvider.IsAdminAsync(requirement.ServerId)
                || await _userProvider.HasClaimsAsync(requirement.ServerId, requirement.ClaimTypes))
            {
                context.Succeed(requirement);
            }
        }
    }
}