using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sparkle.Domain.Messages.ValueObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Domain.Messages;

public class Message
{
    /// <summary>
    /// Unique Id as an string representation of an ObjectId type
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [StringLength(24, MinimumLength = 24)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string Id { get; set; }

    /// <summary>
    /// Message body
    /// </summary>
    [MaxLength(2000)]
    [DefaultValue("Hello! What's your name")]
    public string Text { get; set; }

    /// <summary>
    /// Time when server received message
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// Time when message was pinned
    /// </summary>
    public DateTime? PinnedTime { get; set; } = null;

    /// <summary>
    /// Flag that indicates whether the message is pinned or not
    /// </summary>
    [DefaultValue(false)]
    public bool IsPinned { get; set; } = false;

    /// <summary>
    /// List of reactions to the message
    /// </summary>
    public List<Reaction> Reactions { get; set; } = new();

    /// <summary>
    /// List of message attachment urls
    /// </summary>
    public List<Attachment> Attachments { get; set; } = new();
    /// <summary>
    /// Message author ID
    /// </summary>
    public Guid AuthorProfile { get; set; }
    public Guid Author { get; set; }
    /// <summary>
    /// Chat Id as an string representation of an ObjectId type
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [StringLength(24, MinimumLength = 24)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string ChatId { get; set; }

    public Message()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }
}