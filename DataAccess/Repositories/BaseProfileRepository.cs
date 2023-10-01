using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{

    /// <summary>
    /// Base repository for profiles with CRUD operations.
    /// </summary>
    /// <typeparam name="TProfile">Type of profile entity. Must inherit from <see cref="UserProfile"/> </typeparam>
    public abstract class BaseProfileRepository<TProfile> : BaseSqlRepository<TProfile, Guid>
        where TProfile : UserProfile


    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseProfileRepository{TProfile}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        protected BaseProfileRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Adds a new profile to the database.
        /// </summary>
        /// <param name="profile">The profile to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The added profile.</returns>
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

        /// <summary>
        /// Adds multiple profiles to the database.
        /// </summary>
        /// <param name="profiles">The profiles to add.</param>
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

        /// <summary>
        /// Adds multiple profiles to the database asynchronously.
        /// </summary>
        /// <param name="profiles">The profiles to add.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
