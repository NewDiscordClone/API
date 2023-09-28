using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public abstract class BaseProfileRepository<TProfile> : SimpleSqlDbSet<TProfile>
        where TProfile : UserProfile
    {
        protected BaseProfileRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<TProfile> AddAsync(TProfile entity, CancellationToken cancellationToken = default)
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

        public override void AddMany(IEnumerable<TProfile> entities)
        {
            List<RoleUserProfile> roleUserProfiles = new();

            foreach (TProfile entity in entities)
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

        public override async Task AddManyAsync(IEnumerable<TProfile> entities, CancellationToken cancellationToken = default)
        {
            List<RoleUserProfile> roleUserProfiles = new();

            foreach (TProfile entity in entities)
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
    }
}
