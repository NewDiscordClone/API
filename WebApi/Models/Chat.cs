namespace WebApi.Models;

public class Chat
{
    public int ChatId { get; set; }
    
    public virtual PrivateChat? PrivateChat { get; set; }
    public virtual Channel? Channel { get; set; }
    public virtual List<Message> Messages { get; set; } = new();
}