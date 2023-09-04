using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models
{
    public enum RelationshipType
    {
        Acquaintance,
        Friend,
        Pending,
        Waiting,
        Blocked
    }
    public class RelationshipList
    {
        [BsonId]
        public int Id { get; set; }
        public List<Relationship> Relationships { get; set; }
    }
    public class Relationship
    {
        public int UserId { get; set; }
        public RelationshipType RelationshipType { get; set; }
    }
}