using Microsoft.AspNetCore.Identity;

namespace Sparkle.Domain
{
    public class ServerClaim : IdentityRoleClaim<Guid>
    {
        public override string? ClaimValue { get; set; } = "true";

    }
}