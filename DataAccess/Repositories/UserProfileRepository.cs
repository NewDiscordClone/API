using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class UserProfileRepository : ProfileRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(PostgresDbContext context) : base(context)
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
        public async Task<UserProfile?> FindOrDefaultByChatIdAndUserIdAsync(string chatId,
            Guid userId,
            bool includeRoles = false,
            CancellationToken cancellationToken = default)
        {
            if (includeRoles)
                return await DbSet
                    .Include(profile => profile.Roles)
                    .SingleOrDefaultAsync(profile => profile.ChatId == chatId
                    && profile.UserId == userId,
                    cancellationToken: cancellationToken);

            return await FindOrDefaultByChatIdAndUserIdAsync(chatId, userId, cancellationToken);
        }

        public async Task DeleteGroupChatOwner(Guid profileId, CancellationToken cancellationToken = default)
        {
            RoleUserProfile? currentRecord = await Context.RoleUserProfile
                .FindAsync(new object?[] { Constants.Roles.GroupChatOwnerId, profileId },
                 cancellationToken: cancellationToken)
                ?? throw new EntityNotFoundException(profileId);

            if (currentRecord != null)
            {
                Context.RoleUserProfile.Remove(currentRecord);
            }

            RoleUserProfile oldOwner = new()
            {
                UserProfileId = profileId,
                RolesId = Constants.Roles.GroupChatMemberId
            };

            await Context.RoleUserProfile.AddAsync(oldOwner, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task SetGroupChatOwner(Guid profileId, CancellationToken cancellationToken = default)
        {
            RoleUserProfile? currentRecord = await Context.RoleUserProfile
                .FindAsync(new object[] { Constants.Roles.GroupChatMemberId, profileId },
                    cancellationToken: cancellationToken);

            if (currentRecord != null)
            {
                Context.RoleUserProfile.Remove(currentRecord);
            }

            RoleUserProfile newOwner = new()
            {
                UserProfileId = profileId,
                RolesId = Constants.Roles.GroupChatOwnerId
            };

            await Context.RoleUserProfile.AddAsync(newOwner, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }

    }
}
