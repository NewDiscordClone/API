using System.ComponentModel;

namespace Application.Models;

public class Reaction
{
    [DefaultValue(":smile:")]
    public string Emoji { get; set; }
    public UserLookUp User { get; set; }
}