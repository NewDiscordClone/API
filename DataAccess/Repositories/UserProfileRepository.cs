using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class UserProfileRepository : SimpleSqlDbSet<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(AppDbContext context) : base(context, default)
        {
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
