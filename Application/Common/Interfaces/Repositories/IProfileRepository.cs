using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IProfileRepository<TProfile> : IRepository<TProfile, Guid>
        where TProfile : UserProfile
    {
        Task<TProfile?> FindOrDefaultAsync(Guid id, CancellationToken cancellationToken = default, bool includeRoles = false);
    }
}
