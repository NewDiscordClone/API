using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace Application.Models;

public class Role : IdentityRole<ObjectId>
{
    public string Color { get; set; }
    public ObjectId ServerId { get; set; }
}