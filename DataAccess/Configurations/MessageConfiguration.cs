using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .OnDelete(DeleteBehavior.Cascade); 
            
            builder.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .OnDelete(DeleteBehavior.NoAction); //Розрахунок на те, що юзер не видаляється
        }
    }
}