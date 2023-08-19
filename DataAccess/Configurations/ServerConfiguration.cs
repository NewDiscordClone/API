using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ServerConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasOne(s => s.Owner)
                .WithMany(u => u.OwnedServers)
                .OnDelete(DeleteBehavior.Restrict); 
            builder.HasMany(s => s.ServerProfiles)
                .WithOne(sp => sp.Server)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.Channels)
                .WithOne(sp => sp.Server)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(s => s.Roles)
                .WithOne(sp => sp.Server)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}