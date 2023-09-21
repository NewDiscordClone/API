using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles");

            builder.HasDiscriminator<string>("ProfileType")
              .HasValue<UserProfile>("user")
              .HasValue<ServerProfile>("server");

            builder.Property(p => p.Id)
                .ValueGeneratedNever();
        }
    }
}
