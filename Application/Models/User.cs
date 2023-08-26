using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;

namespace Application.Models;

public enum UserStatus
{
    Online,
    Idle,
    DoNotDisturb,
    Offline
}
public class User : MongoUser<ObjectId>
{
    public string? DisplayName { get; set; }
    public string? Avatar { get; set; }
    public UserStatus Status { get; set; }
    public string? TextStatus { get; set; }
}