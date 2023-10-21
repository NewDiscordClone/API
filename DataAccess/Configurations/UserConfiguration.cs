using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            AddDefaultData(builder);

            builder.HasIndex(user => user.UserName).IsUnique();
        }

        private static void AddDefaultData(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = Constants.User.DefaultUser1Id,
                    UserName = "dnesh1",
                    NormalizedUserName = "DNESH1",
                    Email = "dneshotkin@gmail.com",
                    NormalizedEmail = "DNESHOTKIN@GMAIL.COM",
                    DisplayName = "Grabtot",
                    PasswordHash = "AQAAAAIAAYagAAAAELLPwOPnHPJdFRtP7OCgMMQ4n7IAUrj5F7ZFbvkzbwdA1e5o1BSCxm3zf6pordQ1Ow==",
                    SecurityStamp = "YNSKJ23UUSGYIOOXJTIKUUTHF3FXW43N",
                    ConcurrencyStamp = "9372f0b4-1686-4af9-8962-43ae874b7a6f"
                },
                new User
                {
                    Id = Constants.User.DefaultUser2Id,
                    UserName = "dnesh2",
                    NormalizedUserName = "DNESH2",
                    Email = "dneshotkin@email.com",
                    NormalizedEmail = "DNESHOTKIN@EMAIL.COM",
                    PasswordHash = "AQAAAAIAAYagAAAAEDhRq1TO1+Bt5t+MWYFRaeRu7OrRR8LVhJNio81zmfnaZwdWhwUbHaEuj1vpSOngVg==",
                    SecurityStamp = "KDGJ6POASVRE527NHMQZ4FQPGL4OREWT",
                    ConcurrencyStamp = "a2075b1f-8f89-4df9-9f85-39deaec2b403"
                },
                new User
                {
                    Id = Constants.User.DefaultUser3Id,
                    UserName = "dnesh3",
                    NormalizedUserName = "DNESH3",
                    Email = "dneshotkin@ebail.com",
                    NormalizedEmail = "DNESHOTKIN@EBAIL.COM",
                    DisplayName = "Grabbot",
                    PasswordHash = "AQAAAAIAAYagAAAAECIvg/r9riF/7qS+ETlEE6L+wNUWORELOOYoI78NY+hKBk2/YP4+0F9nZ12cLs57Sw==",
                    SecurityStamp = "X73JA3M4E5VMK35LG7HMINOGF5AWAM5B",
                    ConcurrencyStamp = "93e4df6c-20cc-410b-9082-daac577e184a"
                }
            );
        }
    }
}