using MongoDB.Bson.Serialization.Attributes;

namespace Sparkle.Domain
{
    public class UserConnections
    {
        [BsonId]
        public Guid UserId { get; set; }
        public HashSet<string> Connections { get; set; }
    }
}