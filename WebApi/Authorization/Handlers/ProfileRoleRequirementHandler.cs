using Microsoft.AspNetCore.Authorization;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization.Handlers
{
    public class ProfileRoleRequirementHandler : AuthorizationHandler<ProfileRoleRequirement>
    {
        private readonly IUserProfileRepository _repository;

        public ProfileRoleRequirementHandler(IUserProfileRepository repository)
        {
            _repository = repository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileRoleRequirement requirement)
        {
            UserProfile? profile = await _repository
                .FindOrDefaultAsync(requirement.ProfileId, includeRoles: true);

            if (profile == null)
            {
                return;
            }

            List<Role> userRoles = profile.Roles;

            IEnumerable<string> defaultRoles = requirement.Roles
                .Where(role => Constants.Roles.DefaultRoleNames.Contains(role));

            IEnumerable<string> customRoles = requirement.Roles.Except(defaultRoles);

            if (!requirement.Roles.All(role => userRoles.Any(userRole => userRole.Name == role)))
            {
                return;
            }

            if (requirement.Roles.All(role => userRoles.Any(userRole => userRole.Name == role)))
            {
                context.Succeed(requirement);
            }
        }
    }
}
