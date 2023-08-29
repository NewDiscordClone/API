using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Channel : Chat
{
    public string Title { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ServerId { get; set; }
    
    
}