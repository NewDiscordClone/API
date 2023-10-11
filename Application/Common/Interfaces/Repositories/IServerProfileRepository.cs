using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Interface for a repository that manages server profiles.
    /// </summary>
    public interface IServerProfileRepository : IProfileRepository<ServerProfile>
    {
        ///<summary>
        /// Adds roles to a server profile.
        /// </summary>
        /// <param name="profileId">The ID of the server profile.</param>
        /// <param name="roleIds">The IDs of the roles to add.</param>
        Task AddRolesAsync(Guid profileId, params Guid[] roleIds);

        /// <summary>
        /// Finds a user's server profile on a specific server.
        /// </summary>
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user's server profile, or null if not found.</returns>
        ServerProfile? FindUserProfileOnServer(string serverId, Guid userId);

        /// <summary>
        /// Finds a user's server profile on a specific server asynchronously.
        /// </summary>
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The user's server profile, or null if not found.</returns>
        Task<ServerProfile?> FindUserProfileOnServerAsync(string serverId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the IDs of the roles assigned to a server profile.
        /// </summary>
        /// <param name="profileId">The ID of the server profile.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A list of role IDs.</returns>
        Task<List<Guid>> GetRolesIdsAsync(Guid profileId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the roles assigned to a server profile.
        /// </summary>
        /// <param name="profileId">The ID of the server profile.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A list of roles.</returns>
        Task<List<Role>> GetRolesAsync(Guid profileId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a user is a member of a server.
        /// </summary>
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user is a member of the server, false otherwise.</returns>
        bool IsUserServerMember(string serverId, Guid userId);

        /// <summary>
        /// Removes a role from all server profiles on a specific server.
        /// </summary>
        /// <param name="role">The role to remove.</param>
        /// <param name="serverId">The ID of the server.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        Task RemoveRoleFromServerProfilesAsync(Role role, string serverId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes roles from a server profile.
        /// </summary>
        /// <param name="profileId">The ID of the server profile.</param>
        /// <param name="roleIds">The IDs of the roles to remove.</param>
        Task RemoveRolesAsync(Guid profileId, params Guid[] roleIds);

        /// <summary>
        /// Checks if a server profile is owned by the user associated with it.
        /// </summary>
        /// <param name="profile">The server profile to check.</param>
        /// <returns>True if the server profile is owned by the user associated with it, false otherwise.</returns>
        bool IsServerOwner(ServerProfile profile);

        /// <summary>
        /// Checks if a server profile is owned by the user associated with it.
        /// </summary>
        /// <param name="profileId">The ID of the server profile to check.</param>
        /// <returns>True if the server profile is owned by the user associated with it, false otherwise.</returns>
        bool IsServerOwner(Guid profileId);

        /// <summary>
        /// Checks if a server profile is owned by the user associated with it asynchronously.
        /// </summary>
        /// <param name="profile">The server profile to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the server profile is owned by the user associated with it, false otherwise.</returns>
        Task<bool> IsServerOwnerAsync(ServerProfile profile);

        /// <summary>
        /// Checks if a server profile is owned by the user associated with it asynchronously.
        /// </summary>
        /// <param name="profileId">The ID of the server profile to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the server profile is owned by the user associated with it, false otherwise.</returns>
        Task<bool> IsServerOwnerAsync(Guid profileId);
    }
}
