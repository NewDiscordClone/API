using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization.Requirements
{
    public record ServerClaimsAuthorizationRequirement : IAuthorizationRequirement
    {
        public int ServerId { get; init; }
        public IEnumerable<string> ClaimTypes { get; init; }

        public ServerClaimsAuthorizationRequirement(int serverId, IEnumerable<string> claimTypes)
        {
            ServerId = serverId;
            ClaimTypes = claimTypes;
        }
    }
}