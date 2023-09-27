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
            return DbSet.Any(profile => profile.ChatId == chatId && profile.UserId == userId);
        }

        public async Task<bool> ChatContainsUserAsync(string chatId, Guid userId)
        {
            return await DbSet.AnyAsync(profile => profile.ChatId == chatId && profile.UserId == userId);
        }

        public override async Task<UserProfile> AddAsync(UserProfile entity, CancellationToken cancellationToken = default)
        {
            List<RoleUserProfile> roles = entity.Roles
                .ConvertAll(entity => new RoleUserProfile()
                {
                    RolesId = entity.Id,
                    UserProfileId = entity.Id
                });

            entity.Roles.Clear();

            await Context.RoleUserProfile.AddRangeAsync(roles, cancellationToken);
            await Context.UserProfiles.AddAsync(entity, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public override void AddMany(IEnumerable<UserProfile> entities)
        {
            List<RoleUserProfile> roleUserProfiles = new();

            foreach (UserProfile entity in entities)
            {
                IEnumerable<RoleUserProfile> roles = entity.Roles.Select(role => new RoleUserProfile
                {
                    RolesId = role.Id,
                    UserProfileId = entity.Id
                });

                roleUserProfiles.AddRange(roles);
                entity.Roles.Clear();
            }

            Context.RoleUserProfile.AddRange(roleUserProfiles);
            Context.UserProfiles.AddRangeAsync(entities);

            Context.SaveChanges();
        }

        public override async Task AddManyAsync(IEnumerable<UserProfile> entities, CancellationToken cancellationToken = default)
        {
            List<RoleUserProfile> roleUserProfiles = new();

            foreach (UserProfile entity in entities)
            {
                IEnumerable<RoleUserProfile> roles = entity.Roles.Select(role => new RoleUserProfile
                {
                    RolesId = role.Id,
                    UserProfileId = entity.Id
                });

                roleUserProfiles.AddRange(roles);
                entity.Roles.Clear();
            }

            await Context.RoleUserProfile.AddRangeAsync(roleUserProfiles, cancellationToken);
            await Context.UserProfiles.AddRangeAsync(entities, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);
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
