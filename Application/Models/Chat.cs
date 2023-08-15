using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models;

public abstract class Chat
{
    public int Id { get; set; }
    
    [NotMapped]
    public List<Message> PinnedMessages => Messages.Where(m => m.IsPinned).ToList();
    public virtual List<Message> Messages { get; set; } = new();
    public virtual List<User> Users { get; set; } = new();
}