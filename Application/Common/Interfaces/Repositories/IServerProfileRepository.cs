using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IServerProfileRepository : ISimpleDbSet<ServerProfile, Guid>
    {
        UserProfile? FindUserProfileOnServer(string id, Guid guid, CancellationToken cancellationToken = default);
        Task<UserProfile?> FindUserProfileOnServerAsync(string id, Guid guid);
        bool IsUserServerMember(string serverId, Guid userId);
        Task RemoveRoleFromServerProfilesAsync(Role role, string serverId, CancellationToken cancellationToken = default);
    }
}
