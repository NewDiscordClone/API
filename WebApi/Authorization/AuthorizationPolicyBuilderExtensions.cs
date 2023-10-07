using Microsoft.AspNetCore.Authorization;
using Sparkle.WebApi.Authorization.Requirements;

namespace Sparkle.WebApi.Authorization
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireRoleClaims
            (this AuthorizationPolicyBuilder builder, Guid profileId, params string[] claimTypes)
        {
            return builder.RequireRoleClaims(profileId, (IEnumerable<string>)claimTypes);
        }
        public static AuthorizationPolicyBuilder RequireRoleClaims
            (this AuthorizationPolicyBuilder builder, Guid profileId, IEnumerable<string> claimTypes)
        {
            builder.Requirements.Add(new RoleClaimsAuthorizationRequirement(profileId, claimTypes));
            return builder;
        }
    }
}
