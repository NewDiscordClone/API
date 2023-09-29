using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Users.Queries.GetRelationships
{
    public record RelationshipDto
    {
        /// <summary>
        /// User who has a relationship with the current user
        /// </summary>
        public UserLookUp User { get; set; }

        /// <summary>
        /// Type of relationship
        /// </summary>
        public RelationshipTypes RelationshipType { get; set; }
    }
}