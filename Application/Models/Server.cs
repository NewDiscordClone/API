using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Application.Models;

public class Server
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }

    public UserLookUp Owner { get; set; }
    public List<ServerProfile> ServerProfiles { get; set; } = new();
}