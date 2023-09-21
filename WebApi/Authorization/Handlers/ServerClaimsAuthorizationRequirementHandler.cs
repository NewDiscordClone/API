using Microsoft.AspNetCore.Authorization;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization.Handlers
{
    public class ServerClaimsAuthorizationRequirementHandler :
        AuthorizationHandler<RoleClaimsAuthorizationRequirement>
    {
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IAppDbContext _context;

        public ServerClaimsAuthorizationRequirementHandler(IAuthorizedUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleClaimsAuthorizationRequirement requirement)
        {
            UserProfile? profile = await _context.UserProfiles.FindOrDefaultAsync(requirement.UserProfileId);

            if (profile is null)
                return;

            if (_userProvider.IsAdmin(profile)
                || await _userProvider.HasClaimsAsync(profile, requirement.ClaimTypes))
            {
                context.Succeed(requirement);
            }
        }
    }
}