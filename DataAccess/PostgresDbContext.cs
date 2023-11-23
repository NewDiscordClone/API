using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess
{
    public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<RoleUserProfile> RoleUserProfile { get; set; }
        public DbSet<Relationship> Relationships { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(PostgresDbContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}