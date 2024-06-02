using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Models;
using Sparkle.DataAccess.Configurations;

namespace Sparkle.DataAccess
{
    public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RoleUserProfile> RoleUserProfile { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ClaimsConfiguration());
            builder.ApplyConfiguration(new RelationshipConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());

            base.OnModelCreating(builder);
        }
    }
}