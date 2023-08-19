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
                .OnDelete(DeleteBehavior.Restrict); 
            
            builder.HasOne(m => m.User)
                .WithMany(u => u.Messages)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasMany(m => m.Reactions)
                .WithOne(r => r.Message)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(m => m.Attachments)
                .WithOne(a => a.Message)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}