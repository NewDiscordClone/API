using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class UserProfileRepository : BaseProfileRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext context) : base(context)
        {
        }

        public bool ChatContainsUser(string chatId, Guid userId)
        {
            return DbSet.Any(profile => profile.ChatId == chatId && profile.UserId == userId);
        }

        public async Task<bool> ChatContainsUserAsync(string chatId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(profile => profile.ChatId == chatId
                && profile.UserId == userId,
                cancellationToken);
        }

        public async Task<UserProfile> FindByChatIdAndUserIdAsync(string chatId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet.SingleAsync(profile => profile.ChatId == chatId
                && profile.UserId == userId,
                cancellationToken: cancellationToken);
        }

        public async Task<UserProfile?> FindOrDefaultByChatIdAndUserIdAsync(string chatId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet.SingleOrDefaultAsync(profile => profile.ChatId == chatId
                && profile.UserId == userId,
                cancellationToken: cancellationToken);
        }
    }
}
