using Microsoft.AspNetCore.Authorization;

namespace Sparkle.WebApi.Authorization.Requirements
{
    public class ProfileRoleRequirement : IAuthorizationRequirement
    {
        public Guid ProfileId { get; init; }
        public string[] Roles { get; init; }

        public ProfileRoleRequirement(Guid profileId, string[] roles)
        {
            ProfileId = profileId;
            Roles = roles;
        }
    }
}
