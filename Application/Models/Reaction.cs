namespace Application.Models;

public class Reaction
{
    public int Id { get; set; }
    public string Emoji { get; set; }
    
    public virtual User User { get; set; }
    public virtual Message Message { get; set; }
}