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
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sp => sp.User)
                .WithMany(u => u.ServerProfiles)
                .OnDelete(DeleteBehavior.Cascade); //Розрахунок на те, що юзер не видаляється
            builder.HasMany(sp => sp.Roles)
                .WithMany(u => u.ServerProfiles)
                .UsingEntity<Dictionary<string, object>>(
                "ServerProfileRoles",
                j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<ServerProfile>().WithMany().HasForeignKey("ServerProfileId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey("ServerProfileId", "RoleId"));

        }
    }
}