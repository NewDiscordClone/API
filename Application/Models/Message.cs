using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime SendTime { get; set; }
    public DateTime? PinnedTime { get; set; } = null;
    public bool IsPinned { get; set; } = false;

    public List<Reaction> Reactions { get; set; } = new();
    public List<Attachment> Attachments { get; set; } = new();
    public UserLookUp User { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string ChatId { get; set; }
}