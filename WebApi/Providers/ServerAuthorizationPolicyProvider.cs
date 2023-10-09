using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sparkle.Application.Common.RegularExpressions;
using Sparkle.WebApi.Authorization;
using Sparkle.WebApi.Authorization.Requirements;
using System.Text.RegularExpressions;
using static Sparkle.Application.Common.Constants.Constants;

namespace WebApi.Providers
{
    public class ServerAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private AuthorizationPolicyBuilder _policyBuilder;
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public ServerAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new(options);
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
           await FallbackPolicyProvider.GetDefaultPolicyAsync();

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            await FallbackPolicyProvider.GetFallbackPolicyAsync();


        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policy)
        {
            if (!TryParseProfileId(policy, out Guid profileId))
                throw new InvalidOperationException("No profile id provided");
            _policyBuilder = new();

            if (TryParsePolicyName(policy, out string policyName))
                return await GetDefaultPolicyAsync(policyName, profileId);

            if (TryParseRoles(policy, out string[] roles))
            {
                _policyBuilder.RequireProfileRole(profileId, roles);
            }

            if (TryParseClaims(policy, out string[] claims))
            {
                _policyBuilder.RequireRoleClaims(profileId, claims);
            }

            return _policyBuilder.Build();

        }

        private static bool TryParseClaims(string policy, out string[] claims)
        {
            claims = Array.Empty<string>();

            if (string.IsNullOrEmpty(policy))
            {
                return false;
            }

            Regex claimsRegex = Regexes.Authorization.ClaimsRegex;
            Match match = claimsRegex.Match(policy);
            if (match.Success)
            {
                claims = match.Groups[1].Value.Split(',');
                return true;
            }

            if (claims.Any(claim => !Claims.GetClaims().Contains(claim)))
                throw new InvalidOperationException("Invalid claim");

            return false;
        }

        private static bool TryParseRoles(string policy, out string[] roles)
        {
            roles = Array.Empty<string>();

            if (string.IsNullOrEmpty(policy))
            {
                return false;
            }

            Regex rolesRegex = Regexes.Authorization.RolesRegex;
            Match match = rolesRegex.Match(policy);
            if (match.Success)
            {
                roles = match.Groups[1].Value.Split(',');
                return true;
            }

            return false;
        }

        private static bool TryParsePolicyName(string policy, out string policyName)
        {
            policyName = string.Empty;

            if (string.IsNullOrEmpty(policy))
            {
                return false;
            }

            Regex policyNameRegex = Regexes.Authorization.PolicyRegex;
            Match match = policyNameRegex.Match(policy);
            if (match.Success)
            {
                policyName = match.Groups[1].Value;
                return true;
            }

            return false;
        }

        private static bool TryParseProfileId(string policyName, out Guid profileId)
        {
            profileId = default;

            if (string.IsNullOrEmpty(policyName))
            {
                return false;
            }

            Regex serverIdRegex = Regexes.Authorization.ProfileIdRegex;
            Match match = serverIdRegex.Match(policyName);
            if (match.Success)
            {
                profileId = Guid.Parse(match.Value);
                return true;
            }
            return false;
        }

        private async Task<AuthorizationPolicy?> GetDefaultPolicyAsync(string policyName, Guid profileId)
        {
            switch (policyName)
            {
                case Policies.SendMessages:
                    throw new NotImplementedException();// TODO: Добавьте логику для SendMessages
                case Policies.ChangeProfileName:
                    // выполняться должно хотя бы одно из требований чтобы политика выполнилась
                    RoleClaimsRequirement requirement1 = new(profileId,
                        new List<string> { Claims.ChangeSomeoneServerName });
                    _policyBuilder.RequireRoleClaims(profileId, Claims.ChangeSomeoneServerName);

                    ProfileOwnerRequirement requirement2 = new();

                    _policyBuilder.Requirements.Add(requirement1);
                    _policyBuilder.Requirements.Add(requirement2);
                    return _policyBuilder.Build();
                default:
                    return await FallbackPolicyProvider.GetPolicyAsync(policyName);
            };
        }

        private AuthorizationPolicy GetClaimsPolicy(Guid profileId,
        params string[] claims)
        {
            _policyBuilder.RequireRoleClaims(profileId, claims);
            return _policyBuilder.Build();
        }
        private AuthorizationPolicy GetRolesPolicy(Guid profileId,
        params string[] roles)
        {
            _policyBuilder.RequireProfileRole(profileId, roles);
            return _policyBuilder.Build();
        }
    }
}