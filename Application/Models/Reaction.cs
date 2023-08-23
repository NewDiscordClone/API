namespace Application.Models;

public class Reaction
{
    public string Emoji { get; set; }
    public virtual UserLookUp User { get; set; }
}