using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for creating and managing roles in the application.
    /// </summary>
    public interface IRoleFactory
    {
        Role PersonalChatMemberRole { get; }
        Role GroupChatMemberRole { get; }
        Role GroupChatOwnerRole { get; }
        Role ServerOwnerRole { get; }
        Role ServerMemberRole { get; }
        string[] GroupChatOwnerClaims { get; }
        string[] GroupChatMemberClaims { get; }
        string[] PersonalChatMemberClaims { get; }
        string[] ServerMemberDefaultClaims { get; }
        string[] ServerOwnerDefaultClaims { get; }

        /// <summary>
        /// Creates a new server role with the specified name, color, priority, claims and server ID.
        /// </summary>
        /// <param name="name">The name of the new role.</param>
        /// <param name="color">The color of the new role.</param>
        /// <param name="priority">The priority of the new role.</param>
        /// <param name="claims">The claims associated with the new role.</param>
        /// <param name="serverId">The ID of the server the new role belongs to.</param>
        /// <returns>The newly created role.</returns>
        Task<Role> CreateServerRoleAsync(string name, string color, int priority, string[] claims, string serverId);

        /// <summary>
        /// Creates a new server role with the specified role and claims.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <param name="claims">The claims associated with the new role.</param>
        /// <returns>The newly created role.</returns>
        Task<Role> CreateServerRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims);

        /// <summary>
        /// Creates a new server role with the specified role and claims.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <param name="claims">The claims associated with the new role.</param>
        /// <returns>The newly created role.</returns>
        Task<Role> CreateServerRoleAsync(Role role, string[] claims);

        /// <summary>
        /// Gets the default server roles.
        /// </summary>
        /// <returns>The default server roles.</returns>
        List<Role> GetDefaultServerRoles();

        /// <summary>
        /// Gets the default group chat roles.
        /// </summary>
        /// <returns>The default group chat roles.</returns>
        List<Role> GetGroupChatRoles();
    }
}
