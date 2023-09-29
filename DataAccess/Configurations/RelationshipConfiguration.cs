using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Configurations
{
    public class RelationshipConfiguration : IEntityTypeConfiguration<Relationship>
    {
        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            builder.HasKey(nameof(Relationship.UserActive), nameof(Relationship.UserPassive));
        }
    }
}
