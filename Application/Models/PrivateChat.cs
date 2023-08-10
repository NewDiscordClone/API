namespace Application.Models;

public class PrivateChat : Chat
{
    public string? Image { get; set; }
    public string? Title { get; set; }

    public virtual User Owner { get; set; }
    public virtual List<User> Users { get; set; } = new();
}