using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository : ISimpleDbSet<Role, Guid>
    {
        Task<Role> GetServerMemberRoleAsync(string serverId, CancellationToken cancellationToken = default);
        Task<List<IdentityRoleClaim<Guid>>> GetRoleClaimAsync(Role role, CancellationToken cancellationToken = default);
        Task AddClaimToRoleAsync(Role role, IdentityRoleClaim<Guid> claim, CancellationToken cancellationToken = default);
        Task AddClaimsToRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims, CancellationToken cancellationToken = default);
        Task RemoveClaimsFromRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims, CancellationToken cancellationToken = default);
        Task RemoveClaimsFromRoleAsync(Role role, CancellationToken cancellationToken = default);
        Task RemoveClaimFromRoleAsync(Role role, IdentityRoleClaim<Guid> claim, CancellationToken cancellationToken = default);
        bool IsPriorityUniqueInServer(string serverId, int priority);
    }
}
