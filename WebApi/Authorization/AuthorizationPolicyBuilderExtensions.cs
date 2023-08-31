using Microsoft.AspNetCore.Authorization;
using WebApi.Authorization.Requirements;

namespace WebApi.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireServerClaim
            (this AuthorizationPolicyBuilder builder, string serverId, params string[] claimTypes)
        {
            return RequireServerClaim(builder, serverId, (IEnumerable<string>)claimTypes);
        }
        public static AuthorizationPolicyBuilder RequireServerClaim
            (this AuthorizationPolicyBuilder builder, string serverId, IEnumerable<string> claimTypes)
        {
            builder.Requirements.Add(new ServerClaimsAuthorizationRequirement(serverId, claimTypes));
            return builder;
        }
    }
}
