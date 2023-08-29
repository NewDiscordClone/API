using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Role : IdentityRole<int>
{
    public string Color { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string ServerId { get; set; }
}