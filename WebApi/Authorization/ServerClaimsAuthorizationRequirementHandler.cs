using Application.Providers;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization
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
            if (await _userProvider.HasClaimsAsync(requirement.ServerId, requirement.ClaimTypes))
            {
                context.Succeed(requirement);
            }
        }
    }
}