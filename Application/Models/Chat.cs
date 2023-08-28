using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Application.Models;

public abstract class Chat
{
    [BsonId]
    public ObjectId Id { get; set; }
    public List<UserLookUp> Users { get; set; } = new();
}