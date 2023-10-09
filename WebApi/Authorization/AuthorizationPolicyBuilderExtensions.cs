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
            builder.Requirements.Add(new RoleClaimsRequirement(profileId, claimTypes));
            return builder;
        }

        public static AuthorizationPolicyBuilder RequireProfileRole(this AuthorizationPolicyBuilder builder,
            Guid profileId, IEnumerable<string> roles)
        {
            builder.Requirements.Add(new ProfileRoleRequirement(profileId, roles.ToArray()));
            return builder;
        }
        public static AuthorizationPolicyBuilder RequireProfileRole(this AuthorizationPolicyBuilder builder,
            Guid profileId, params string[] roles)
        {
            return builder.RequireProfileRole(profileId, (IEnumerable<string>)roles);
        }
    }
}
