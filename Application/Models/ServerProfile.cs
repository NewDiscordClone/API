#nullable enable
namespace Application.Models;

public class ServerProfile
{
    public int Id { get; set; }
    public string? DisplayName { get; set; }
    
    public virtual User User { get; set; }
    public virtual Server Server { get; set; }
    public virtual List<Role> Roles { get; set; }
}