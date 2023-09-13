using Application.Models;
using Application.Models.LookUps;

namespace Application.Users.Queries.GetRelationships
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
        public RelationshipType RelationshipType { get; set; }
    }
}