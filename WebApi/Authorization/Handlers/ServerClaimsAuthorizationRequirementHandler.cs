using Microsoft.AspNetCore.Authorization;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization.Handlers
{
    public class ServerClaimsAuthorizationRequirementHandler :
        AuthorizationHandler<RoleClaimsRequirement>
    {
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly Application.Common.Interfaces.Repositories.IUserProfileRepository _repository;

        public ServerClaimsAuthorizationRequirementHandler(IAuthorizedUserProvider userProvider, Application.Common.Interfaces.Repositories.IUserProfileRepository repository)
        {
            _userProvider = userProvider;
            _repository = repository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleClaimsRequirement requirement)
        {
            UserProfile? profile = await _repository
                .FindOrDefaultAsync(requirement.UserProfileId, includeRoles: true);

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