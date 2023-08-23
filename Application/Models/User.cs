using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public enum UserStatus
{
    Online,
    Idle,
    DoNotDisturb,
    Offline
}
public class User : IdentityUser<int>
{
    public string? DisplayName { get; set; }
    //public string? AvatarPath { get; set; }
    public UserStatus Status { get; set; }
    public string? TextStatus { get; set; }

    public virtual List<Server> OwnedServers { get; set; } = new();
    public virtual List<ServerProfile> ServerProfiles { get; set; } = new();
}