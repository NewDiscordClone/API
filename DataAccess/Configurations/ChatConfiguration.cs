using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasDiscriminator<string[]>("_t")
              .HasValue<Channel>(["Chat", "Channel"])
              .HasValue<GroupChat>(["Chat", "PersonalChat", "GroupChat"])
              .HasValue<PersonalChat>(["Chat", "PersonalChat"]);
        }
    }
}
