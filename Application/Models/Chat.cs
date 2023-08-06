#nullable enable
namespace Application.Models;

public abstract class Chat
{
    public int Id { get; set; }
    public virtual List<Message> Messages { get; set; } = new();
}