using Microsoft.AspNetCore.Authorization;

namespace Sparkle.WebApi.Authorization.Requirements
{
    public record RoleClaimsRequirement : IAuthorizationRequirement
    {
        public Guid UserProfileId { get; init; }
        public IEnumerable<string> ClaimTypes { get; init; }

        public RoleClaimsRequirement(Guid userProfileId, IEnumerable<string> claimTypes)
        {
            UserProfileId = userProfileId;
            ClaimTypes = claimTypes;
        }
    }
}