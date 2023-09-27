using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class ServerProfileRepository : SimpleSqlDbSet<ServerProfile>, IServerProfileRepository
    {
        //private new DbSet<UserProfile> DbSet { get; }
        public ServerProfileRepository(AppDbContext context) : base(context)
        {
            // DbSet = Context.Set<UserProfile>();
        }

        public async Task RemoveRolesAsync(Guid profileId, params Guid[] roleIds)
        {
            DbSet<RoleUserProfile> roleUserProfile = Context.Set<RoleUserProfile>();

            List<RoleUserProfile> rolesToRemove = await roleUserProfile
                .Where(ru => roleIds.Contains(ru.RolesId) && ru.UserProfileId == profileId)
                .ToListAsync();

            roleUserProfile.RemoveRange(rolesToRemove);

            await Context.SaveChangesAsync();
        }

        public async Task AddRolesAsync(Guid profileId, params Guid[] roleIds)
        {
            DbSet<RoleUserProfile> roleUserProfile = Context.Set<RoleUserProfile>();

            List<RoleUserProfile> rolesToAdd = roleIds.ToList()
                .ConvertAll(roleId => new RoleUserProfile
                {
                    RolesId = roleId,
                    UserProfileId = profileId
                });

            await roleUserProfile.AddRangeAsync(rolesToAdd);

            await Context.SaveChangesAsync();
        }
        public ServerProfile? FindUserProfileOnServer(string serverId, Guid userId)
        {
            return DbSet.SingleOrDefault(profile => profile.ServerId
            == serverId && profile.UserId == userId);
        }

        public async Task<ServerProfile?> FindUserProfileOnServerAsync(string serverId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await DbSet.SingleOrDefaultAsync(profile => profile.ServerId == serverId
                && profile.UserId == userId, cancellationToken);
        }

        public bool IsUserServerMember(string serverId, Guid userId)
        {
            return DbSet.Any(profile => profile.ServerId == serverId && profile.Id == userId);
        }

        public async Task RemoveRoleFromServerProfilesAsync(Role role, string serverId, CancellationToken cancellationToken = default)
        {
            await DbSet.Where(profile => profile.ServerId == serverId && profile.Roles.Contains(role))
                 .ForEachAsync(profile => profile.Roles.Remove(role), cancellationToken);
        }

        public async Task<List<Guid>> GetRolesIdsAsync(Guid profileId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(profile => profile.Id == profileId)
                .SelectMany(profile => profile.Roles)
                .Select(role => role.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Role>> GetRolesAsync(Guid profileId, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(profile => profile.Id == profileId)
                .SelectMany(profile => profile.Roles)
                .ToListAsync(cancellationToken);
        }
    }
}
