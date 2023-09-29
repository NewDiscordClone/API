using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public abstract class BaseProfileRepository<TProfile> : BaseSqlRepository<TProfile, Guid>
        where TProfile : UserProfile
    {
        protected BaseProfileRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Adds a user profile to the database with the associated roles.
        /// </summary>
        /// <param name="profile">The user profile to add.</param>
        /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
        /// <returns>The added user profile.</returns>
        public override async Task<TProfile> AddAsync(TProfile profile, CancellationToken cancellationToken = default)
        {
            Role[] profileRoles = new Role[profile.Roles.Count];

            profile.Roles.CopyTo(profileRoles);

            IEnumerable<RoleUserProfile> roles = profileRoles
                .Select(role => new RoleUserProfile()
                {
                    RolesId = role.Id,
                    UserProfileId = profile.Id
                });

            profile.Roles.Clear();

            await Context.UserProfiles.AddAsync(profile, cancellationToken);
            await Context.RoleUserProfile.AddRangeAsync(roles, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);

            profile.Roles.AddRange(profileRoles);

            return profile;
        }

        public override void AddMany(IEnumerable<TProfile> profiles)
        {
            List<RoleUserProfile> roleUserProfiles = new();

            foreach (TProfile profile in profiles)
            {
                IEnumerable<RoleUserProfile> roles = profile.Roles.Select(role => new RoleUserProfile
                {
                    RolesId = role.Id,
                    UserProfileId = profile.Id
                });

                roleUserProfiles.AddRange(roles);
                profile.Roles.Clear();
            }

            Context.RoleUserProfile.AddRange(roleUserProfiles);
            Context.UserProfiles.AddRangeAsync(profiles);

            Context.SaveChanges();
        }

        public override async Task AddManyAsync(IEnumerable<TProfile> profiles, CancellationToken cancellationToken = default)
        {
            List<RoleUserProfile> roleUserProfiles = new();

            foreach (TProfile profile in profiles)
            {
                IEnumerable<RoleUserProfile> roles = profile.Roles.Select(role => new RoleUserProfile
                {
                    RolesId = role.Id,
                    UserProfileId = profile.Id
                });

                roleUserProfiles.AddRange(roles);
                profile.Roles.Clear();
            }

            await Context.RoleUserProfile.AddRangeAsync(roleUserProfiles, cancellationToken);
            await Context.UserProfiles.AddRangeAsync(profiles, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
