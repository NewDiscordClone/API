using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Channel : Chat
{
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string Title { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ServerId { get; set; }
    
    
}