using Sparkle.Application.Models;
using System.Security.Claims;

namespace Sparkle.Application.Common.Interfaces
{
    /// <summary>
    /// Provides functionality to retrieve and manipulate information about an authorized user.
    /// </summary>
    public interface IAuthorizedUserProvider
    {
        /// <summary>
        /// Gets the unique identifier of the current authorized user.
        /// </summary>
        /// <returns>The unique identifier of the current authorized user.</returns>
        Guid GetUserId();

        /// <summary>
        /// Sets the current authorized user to the specified ClaimsPrincipal.
        /// </summary>
        /// <param name="user">The ClaimsPrincipal representing the current authorized user.</param>
        void SetUser(ClaimsPrincipal user);

        /// <summary>
        /// Determines whether the specified UserProfile is an administrator.
        /// </summary>
        /// <param name="profile">The UserProfile to check.</param>
        /// <returns>True if the UserProfile is an administrator; otherwise, false.</returns>
        bool IsAdmin(UserProfile profile);

        /// <summary>
        /// Determines whether the specified UserProfile has all of the specified claims.
        /// </summary>
        /// <param name="profile">The UserProfile to check.</param>
        /// <param name="claimTypes">The claims to check for.</param>
        /// <returns>True if the UserProfile has all of the specified claims; otherwise, false.</returns>
        bool HasClaims(UserProfile profile, IEnumerable<string> claimTypes);

        /// <summary>
        /// Determines whether the specified UserProfile has all of the specified claims.
        /// </summary>
        /// <param name="profile">The UserProfile to check.</param>
        /// <param name="claimTypes">The claims to check for.</param>
        /// <returns>True if the UserProfile has all of the specified claims; otherwise, false.</returns>
        bool HasClaims(UserProfile profile, params string[] claimTypes);

        /// <summary>
        /// Asynchronously determines whether the specified UserProfile has all of the specified claims.
        /// </summary>
        /// <param name="profile">The UserProfile to check.</param>
        /// <param name="claimTypes">The claims to check for.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains true if the UserProfile has all of the specified claims; otherwise, false.</returns>
        Task<bool> HasClaimsAsync(UserProfile profile, IEnumerable<string> claimTypes);

        /// <summary>
        /// Asynchronously determines whether the specified UserProfile has all of the specified claims.
        /// </summary>
        /// <param name="profile">The UserProfile to check.</param>
        /// <param name="claimTypes">The claims to check for.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains true if the UserProfile has all of the specified claims; otherwise, false.</returns>
        Task<bool> HasClaimsAsync(UserProfile profile, params string[] claimTypes);
    }
}