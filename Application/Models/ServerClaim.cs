using Microsoft.AspNetCore.Identity;

namespace Sparkle.Application.Models
{
    public class ServerClaim : IdentityRoleClaim<Guid>
    {
        public new bool ClaimValue { get; set; } = true;

    }
}