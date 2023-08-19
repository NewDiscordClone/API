using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasOne(r => r.Server)
                .WithMany(s => s.Roles)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.ServerProfiles)
                .WithMany(s => s.Roles);
            
        }
    }
}