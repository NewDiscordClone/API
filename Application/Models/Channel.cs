using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models;

public class Channel : Chat
{
    private List<ServerProfile> _profiles = new();

    [DefaultValue("Test Channel")]
    public string Title { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [StringLength(24, MinimumLength = 24)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string ServerId { get; set; }
    public List<ServerProfile> ServerProfiles { get => _profiles; init => _profiles = value; }
    public override List<UserProfile> Profiles { get => _profiles.ConvertAll(profile => profile as UserProfile); }
}