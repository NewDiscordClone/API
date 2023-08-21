namespace Application.Models;

public class Server
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }
    public virtual List<ServerProfile> ServerProfiles { get; set; } = new();
    public virtual List<Channel> Channels { get; set; } = new();
    public virtual List<Role> Roles { get; set; } = new();
}