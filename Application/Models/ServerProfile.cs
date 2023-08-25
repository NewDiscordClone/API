namespace Application.Models;

public class ServerProfile
{
    public string? DisplayName { get; set; }
    
    public virtual UserLookUp User { get; set; }
    public virtual List<Role> Roles { get; set; }
}