using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasOne(r => r.Message)
                .WithMany(m => m.Reactions)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .OnDelete(DeleteBehavior.NoAction); //Розрахунок на те, що юзер не видаляється
        }
    }
}