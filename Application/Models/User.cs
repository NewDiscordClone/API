#nullable enable
namespace WebApi.Models;

public enum UserStatus
{
    Online,
    Idle,
    DoNotDisturb,
    Offline
}
public class User
{
    public int Id { get; set; }
    public string? DisplayName { get; set; }
    public string? AvatarPath { get; set; }
    public UserStatus Status { get; set; }
    public string? TextStatus { get; set; }

    public virtual List<ServerProfile> ServerProfiles { get; set; } = new();
    public virtual List<Message> Messages { get; set; } = new();
    public virtual List<PrivateChat> PrivateChats { get; set; } = new();
    public virtual List<Reaction> Reactions { get; set; } = new();
}