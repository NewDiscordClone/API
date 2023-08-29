using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Role : IdentityRole<int>
{
    public string Color { get; set; }

    /// <summary>
    /// Server Id as an string representation of an ObjectId type
    /// </summary>
    /// <example>5f95a3c3d0ddad0017ea9291</example>
    [BsonRepresentation(BsonType.ObjectId)]
    public string ServerId { get; set; }
    public bool IsAdmin { get; set; }
}