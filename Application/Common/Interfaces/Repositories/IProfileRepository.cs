using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IProfileRepository<TProfile> : IRepository<TProfile, Guid>
        where TProfile : UserProfile
    {
        Task<TProfile> FindAsync(Guid id, bool includeRoles = false, CancellationToken cancellationToken = default);
        Task<TProfile?> FindOrDefaultAsync(Guid id, CancellationToken cancellationToken = default, bool includeRoles = false);
        Task<List<Guid>> GetUserIdsFromChat(string chatId, CancellationToken cancellationToken = default);
        Task<List<Guid>> GetUserIdsFromServer(string serverId, CancellationToken cancellationToken = default);
    }
}
