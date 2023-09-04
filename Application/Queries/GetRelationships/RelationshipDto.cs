using Application.Models;

namespace Application.Queries.GetRelationships
{
    public class RelationshipDto
    {
        public UserLookUp User { get; set; }
        public RelationshipType RelationshipType { get; set; }
    }
}