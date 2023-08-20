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
                .OnDelete(DeleteBehavior.NoAction); //Розрахунок на те, що юзер не видаляється
        }
    }
}