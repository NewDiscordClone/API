using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using WebApi.Authorization.Requirements;

namespace WebApi.Authorization.Handlers
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