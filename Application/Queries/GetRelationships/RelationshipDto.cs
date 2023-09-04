using Application.Models;
using Application.Models.LookUps;

namespace Application.Queries.GetRelationships
{
    public class RelationshipDto
    {
        public UserLookUp User { get; set; }
        public RelationshipType RelationshipType { get; set; }
    }
}