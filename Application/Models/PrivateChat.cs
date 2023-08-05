#nullable enable
namespace WebApi.Models;

public class PrivateChat: Chat
{
    public string? Image { get; set; }
    public string? Title { get; set; }
    
    public virtual List<User> Users { get; set; } = new();
}