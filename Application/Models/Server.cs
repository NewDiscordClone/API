using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Server
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
    public string Title { get; set; }
    public string? Image { get; set; }

    public UserLookUp Owner { get; set; }
    public List<ServerProfile> ServerProfiles { get; set; } = new();

    public List<Guid> BannedUsers { get; set; } = new();
}