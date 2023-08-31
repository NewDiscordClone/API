using Microsoft.AspNetCore.Authorization;

namespace WebApi.Authorization.Requirements
{
    public record ServerMemberRequirement : IAuthorizationRequirement
    {
        public string ServerId { get; init; }

        public ServerMemberRequirement(string serverId)
        {
            ServerId = serverId;
        }
    }
}
