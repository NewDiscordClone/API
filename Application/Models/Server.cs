using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

/// <summary>
/// Representation of server
/// </summary>
public class Server
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
    /// Title of the server
    /// </summary>
    [Required]
    [StringLength(32, MinimumLength = 1)]
    [DefaultValue("Server 1")]
    public string Title { get; set; }
    /// <summary>
    ///  Image of the server
    /// </summary>
    [RegularExpression(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\/api\/media\/[a-z0-9]{24}$")]
    [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
    public string? Image { get; set; }

    /// <summary>
    ///  User that created server
    /// </summary>
    public Guid Owner { get; set; }

    /// <summary>
    /// List of users profiles on server
    /// </summary>
    public List<ServerProfile> ServerProfiles { get; set; } = new();

    public List<Guid> BannedUsers { get; set; } = new();
}