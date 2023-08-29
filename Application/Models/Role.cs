using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public class Role : IdentityRole<int>
{
    public string Color { get; set; }

    public virtual Server Server { get; set; }
    public bool IsAdmin { get; set; }
    public virtual List<ServerProfile> ServerProfiles { get; set; } = new();
}