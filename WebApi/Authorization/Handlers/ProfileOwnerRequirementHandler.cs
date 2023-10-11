using Microsoft.AspNetCore.Authorization;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization.Handlers
{
    public class ProfileOwnerRequirementHandler : AuthorizationHandler<ProfileOwnerRequirement>
    {
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IUserProfileRepository _profileRepository;
        public ProfileOwnerRequirementHandler(IAuthorizedUserProvider userProvider, IUserProfileRepository profileRepository)
        {
            _userProvider = userProvider;
            _profileRepository = profileRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileOwnerRequirement requirement)
        {
            Guid userId = _userProvider.GetUserId();

            UserProfile profile = await _profileRepository.FindAsync(requirement.ProfileId);

            if (profile.UserId == userId)
                context.Succeed(requirement);
        }
    }
}
