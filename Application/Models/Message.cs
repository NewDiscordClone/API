#nullable enable
namespace WebApi.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime SendTime { get; set; }

    public virtual List<Reaction> Reactions { get; set; } = new();
    public virtual List<Attachment> Attachments { get; set; } = new();
    public virtual User User { get; set; }
    public virtual Chat Chat { get; set; }
    public virtual Message? Reply { get; set; }
}