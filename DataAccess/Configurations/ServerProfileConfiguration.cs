using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ServerProfileConfiguration : IEntityTypeConfiguration<ServerProfile>
    {
        public void Configure(EntityTypeBuilder<ServerProfile> builder)
        {
            builder.HasOne(sp => sp.Server)
                .WithMany(s => s.ServerProfiles)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sp => sp.User)
                .WithMany(u => u.ServerProfiles)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(sp => sp.Roles)
                .WithMany(u => u.ServerProfiles);

        }
    }
}