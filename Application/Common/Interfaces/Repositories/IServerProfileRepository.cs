using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IServerProfileRepository : ISimpleDbSet<ServerProfile, Guid>
    {
        bool IsUserServerMember(string serverId, Guid userId);
        Task RemoveRoleFromServerProfilesAsync(Role role, string serverId, CancellationToken cancellationToken = default);
    }
}
