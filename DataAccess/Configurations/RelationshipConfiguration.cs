using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Common.Constants;
using Sparkle.Domain;

namespace Sparkle.DataAccess.Configurations
{
    public class RelationshipConfiguration : IEntityTypeConfiguration<Relationship>
    {
        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            builder.HasKey(nameof(Relationship.Active), nameof(Relationship.Passive));

            builder.HasData(
                new Relationship
                {
                    Active = Constants.User.DefaultUser1Id,
                    Passive = Constants.User.DefaultUser2Id,
                    RelationshipType = RelationshipTypes.Friend
                },
                new Relationship
                {
                    Active = Constants.User.DefaultUser2Id,
                    Passive = Constants.User.DefaultUser3Id,
                    RelationshipType = RelationshipTypes.Pending
                },
                new Relationship
                {
                    Active = Constants.User.DefaultUser1Id,
                    Passive = Constants.User.DefaultUser3Id,
                    RelationshipType = RelationshipTypes.Friend
                });
        }
    }
}
