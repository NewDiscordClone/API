using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IUserProfileRepository : ISimpleDbSet<UserProfile, Guid>
    {
        bool ChatContainsUser(string chatId, Guid userId);
        Task<bool> ChatContainsUserAsync(string chatId, Guid userId);
        Task<UserProfile> FindByChatIdAndUserIdAsync(string chatId, Guid userId);
        Task<UserProfile?> FindOrDefaultByChatIdAndUserIdAsync(string chatId, Guid userId);
    }
}