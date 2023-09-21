using Microsoft.AspNetCore.Authorization;

namespace Sparkle.WebApi.Authorization.Requirements
{
    public record RoleClaimsAuthorizationRequirement : IAuthorizationRequirement
    {
        public string UserProfileId { get; init; }
        public IEnumerable<string> ClaimTypes { get; init; }

        public RoleClaimsAuthorizationRequirement(string userProfileId, IEnumerable<string> claimTypes)
        {
            UserProfileId = userProfileId;
            ClaimTypes = claimTypes;
        }
    }
}