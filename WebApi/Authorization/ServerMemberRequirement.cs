using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization
{
    public record ServerMemberRequirement : IAuthorizationRequirement
    {
        public int ServerId { get; init; }

        public ServerMemberRequirement(int serverId)
        {
            ServerId = serverId;
        }
    }
}
