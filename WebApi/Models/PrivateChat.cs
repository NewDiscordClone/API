namespace WebApi.Models;

public class PrivateChat
{
    public int Id { get; set; }
    public string? Image { get; set; }
    public string? Title { get; set; }
    
    public virtual Chat Chat {get; set; }
    public virtual List<User> Users { get; set; } = new();
}