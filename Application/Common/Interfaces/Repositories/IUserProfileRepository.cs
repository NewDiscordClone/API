using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
        bool ChatContainsUser(string chatId, Guid userId);
        Task<bool> ChatContainsUserAsync(string chatId, Guid userId, CancellationToken cancellationToken = default);
        Task DeleteGroupChatOwner(Guid profileId, CancellationToken cancellationToken = default);
        Task<UserProfile> FindByChatIdAndUserIdAsync(string chatId, Guid userId, CancellationToken cancellationToken = default);
        Task<UserProfile?> FindOrDefaultByChatIdAndUserIdAsync(string chatId, Guid userId, CancellationToken cancellationToken = default);
        Task SetGroupChatOwner(Guid profileId, CancellationToken cancellationToken = default);
    }
}