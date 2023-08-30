using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization.Requirements
{
    public record ServerClaimsAuthorizationRequirement : IAuthorizationRequirement
    {
        public string ServerId { get; init; }
        public IEnumerable<string> ClaimTypes { get; init; }

        public ServerClaimsAuthorizationRequirement(string serverId, IEnumerable<string> claimTypes)
        {
            ServerId = serverId;
            ClaimTypes = claimTypes;
        }
    }
}