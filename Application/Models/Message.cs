using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Message
{
    /// <summary>
    /// Unique Id as an string representation of an ObjectId type
    /// </summary>
    /// <example>5f95a3c3d0ddad0017ea9291</example>
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
    
    /// <summary>
    /// Chat Id as an string representation of an ObjectId type
    /// </summary>
    /// <example>5f95a3c3d0ddad0017ea9291</example>
    [BsonRepresentation(BsonType.ObjectId)]
    public string ChatId { get; set; }
}