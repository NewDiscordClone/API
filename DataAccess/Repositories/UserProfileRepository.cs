using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class UserProfileRepository : SimpleSqlDbSet<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext context) : base(context)
        {
        }

        public bool ChatContainsUser(string chatId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChatContainsUserAsync(string chatId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> FindByChatIdAndUserIdAsync(string chatId, Guid userId)
        {
            return await DbSet.SingleAsync(profile => profile.ChatId == chatId && profile.UserId == userId);
        }

        public async Task<UserProfile?> FindOrDefaultByChatIdAndUserIdAsync(string chatId, Guid userId)
        {
            return await DbSet.SingleOrDefaultAsync(profile => profile.ChatId == chatId && profile.UserId == userId);
        }
    }
}
