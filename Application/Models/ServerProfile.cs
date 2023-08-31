using System.ComponentModel;

namespace Application.Models;

public class ServerProfile
{
    [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
    public string? DisplayName { get; set; }
    public virtual UserLookUp User { get; set; }
    
    public virtual List<Role> Roles { get; set; }
}