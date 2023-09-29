using Microsoft.AspNetCore.Identity;

namespace Sparkle.Application.Models
{
    public class ServerClaim : IdentityRoleClaim<Guid>
    {
        public override string? ClaimValue { get; set; } = "true";

    }
}