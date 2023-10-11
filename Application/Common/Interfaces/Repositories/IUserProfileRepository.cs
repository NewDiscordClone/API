using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Interface for a repository that manages user profiles.
    /// </summary>
    public interface IUserProfileRepository : IProfileRepository<UserProfile>
    {
        /// <summary>
        /// Checks if a chat contains a user.
        /// </summary>
        /// <param name="chatId">The ID of the chat to check.</param>
        /// <param name="userId">The ID of the user to check for.</param>
        /// <returns>True if the chat contains the user, false otherwise.</returns>
        bool ChatContainsUser(string chatId, Guid userId);

        /// <summary>
        /// Asynchronously checks if a chat contains a user.
        /// </summary>
        /// <param name="chatId">The ID of the chat to check.</param>
        /// <param name="userId">The ID of the user to check for.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains true if the chat contains the user, false otherwise.</returns>
        Task<bool> ChatContainsUserAsync(string chatId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the group chat owner.
        /// </summary>
        /// <param name="profileId">The ID of the user profile to delete the group chat owner for.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteGroupChatOwner(Guid profileId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a user profile by chat ID and user ID.
        /// </summary>
        /// <param name="chatId">The ID of the chat to find the user profile for.</param>
        /// <param name="userId">The ID of the user to find the user profile for.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, null otherwise.</returns>
        Task<UserProfile> FindByChatIdAndUserIdAsync(string chatId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a user profile by chat ID and user ID, or returns null if not found.
        /// </summary>
        /// <param name="chatId">The ID of the chat to find the user profile for.</param>
        /// <param name="userId">The ID of the user to find the user profile for.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user profile if found, null otherwise.</returns>
        Task<UserProfile?> FindOrDefaultByChatIdAndUserIdAsync(string chatId, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the group chat owner.
        /// </summary>
        /// <param name="profileId">The ID of the user profile to set the group chat owner for.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetGroupChatOwner(Guid profileId, CancellationToken cancellationToken = default);
    }
}