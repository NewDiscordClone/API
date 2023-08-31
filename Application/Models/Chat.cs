using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Application.Models;

public abstract class Chat
{
    /// <summary>
    /// Unique Id as an string representation of an ObjectId type
    /// </summary>
    /// <example>5f95a3c3d0ddad0017ea9291</example>
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [StringLength(24, MinimumLength = 24)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string Id { get; set; }
    public List<UserLookUp> Users { get; set; } = new();
}