using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models;

public class Role : IdentityRole<int>
{
    [DefaultValue(1)]
    public override int Id { get; set; }
    [DefaultValue("Admin")]
    public override string Name { get; set; }
    [DefaultValue("#FF0000")]
    public string Color { get; set; }

    /// <summary>
    /// Server Id as an string representation of an ObjectId type
    /// </summary>
    /// <example>5f95a3c3d0ddad0017ea9291</example>
    [BsonRepresentation(BsonType.ObjectId)]
    [StringLength(24, MinimumLength = 24)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string ServerId { get; set; }
    public bool IsAdmin { get; set; }
    public int Priority { get; set; }
}