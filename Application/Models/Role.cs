using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Sparkle.Application.Models;

public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }

    [DefaultValue("Admin")]
    public new string Name { get; set; }

    [DefaultValue("#FF0000")]
    public string Color { get; set; }

    /// <summary>
    /// Server Id as an string representation of an ObjectId type if role is server role
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string? ServerId { get; set; }

    /// <summary>
    /// Admin flag. True if role gives admin permissions 
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// Gets or sets the priority of the role. Higher priority roles take precedence in permission checks.
    /// </summary>
    public int Priority { get; set; }
    public Role()
    {
        Id = Guid.NewGuid();
    }
}