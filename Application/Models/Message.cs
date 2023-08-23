using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Message
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Text { get; set; }
    public DateTime SendTime { get; set; }
    public bool IsPinned { get; set; } = false;

    public List<Reaction> Reactions { get; set; } = new();
    public List<Attachment> Attachments { get; set; } = new();
    public UserLookUp User { get; set; }
    public ObjectId ChatId { get; set; }
}