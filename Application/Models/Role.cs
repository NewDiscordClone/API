using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }
    
    [DefaultValue("Admin")]
    [StringLength(32, MinimumLength = 1)]
    public override string Name { get; set; }

    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$", ErrorMessage = "Color must be in #RRGGBB format")]
    [DefaultValue("#FF0000")]
    public string Color { get; set; }

    /// <summary>
    /// Server Id as an string representation of an ObjectId type
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    [StringLength(24, MinimumLength = 24)]
    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string ServerId { get; set; }

    /// <summary>
    /// Admin flag. True if role gives admin permissions 
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// Gets or sets the priority of the role. Higher priority roles take precedence in permission checks.
    /// </summary>
    public int Priority { get; set; }
}