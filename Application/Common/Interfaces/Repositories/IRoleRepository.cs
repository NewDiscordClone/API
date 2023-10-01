using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Interface for a repository that handles roles.
    /// </summary>
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        /// <summary>
        /// Gets the server member role asynchronously.
        /// </summary>
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the server member role.</returns>
        Task<Role> GetServerMemberRoleAsync(string serverId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the role claim asynchronously.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of identity role claims.</returns>
        Task<List<IdentityRoleClaim<Guid>>> GetRoleClaimAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a claim to a role asynchronously.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="claim">The identity role claim.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddClaimToRoleAsync(Role role, IdentityRoleClaim<Guid> claim, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple claims to a role asynchronously.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="claims">The identity role claims.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddClaimsToRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes multiple claims from a role asynchronously.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="claims">The identity role claims.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveClaimsFromRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes all claims from a role asynchronously.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveClaimsFromRoleAsync(Role role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a claim from a role asynchronously.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="claim">The identity role claim.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveClaimFromRoleAsync(Role role, IdentityRoleClaim<Guid> claim, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether the priority is unique in the server.
        /// </summary>
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>True if the priority is unique in the server, otherwise false.</returns>
        bool IsPriorityUniqueInServer(string serverId, int priority);
    }
}
