using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IServerProfileRepository : ISimpleDbSet<ServerProfile, Guid>
    {
        Task AddRolesAsync(Guid profileId, params Guid[] roleIds);
        ServerProfile? FindUserProfileOnServer(string serverId, Guid userId);
        Task<ServerProfile?> FindUserProfileOnServerAsync(string serverId, Guid userId, CancellationToken cancellationToken = default);
        Task<List<Guid>> GetRolesIdsAsync(Guid profileId, CancellationToken cancellationToken = default);
        Task<List<Role>> GetRolesAsync(Guid profileId, CancellationToken cancellationToken = default);
        bool IsUserServerMember(string serverId, Guid userId);
        Task RemoveRoleFromServerProfilesAsync(Role role, string serverId, CancellationToken cancellationToken = default);
        Task RemoveRolesAsync(Guid profileId, params Guid[] roleIds);
    }
}
