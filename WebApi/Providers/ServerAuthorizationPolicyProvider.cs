using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using WebApi.Authorization;

namespace WebApi.Providers
{
    public partial class ServerAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public ServerAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new(options);
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
           await FallbackPolicyProvider.GetDefaultPolicyAsync();

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            await FallbackPolicyProvider.GetFallbackPolicyAsync();


        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!TryParsePolicyType(policyName, out ServerPolicies policyType))
                return await FallbackPolicyProvider.GetPolicyAsync(policyName);

            if (!TryParseServerId(policyName, out int serverId))
                throw new InvalidOperationException("No server id provided");

            AuthorizationPolicyBuilder policy = new();

            switch (policyType)
            {
                case ServerPolicies.SendMessages:
                case ServerPolicies.ServerMember:
                    policy.AddRequirements(new ServerMemberRequirement(serverId));
                    return policy.Build();
                case ServerPolicies.ManageMessages:
                    policy.RequireServerClaim(serverId, ServerClaims.ManageMessages);
                    return policy.Build();
                default:
                    return await FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
        }

        private bool TryParseServerId(string policyName, out int serverId)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                serverId = default;
                return false;
            }

            Regex serverIdRegex = GetServerIdRegex();
            string idString = serverIdRegex.Match(policyName).Value;
            if (int.TryParse(idString, out serverId))
            {
                return true;
            }
            return false;
        }

        private static bool TryParsePolicyType(string policyName, out ServerPolicies policyType)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                policyType = default;
                return false;
            }

            Regex policyNameRegex = GetPolicyNameRegex();
            string name = policyNameRegex.Match(policyName).Value;
            if (Enum.TryParse(name, true, out policyType))
            {
                return true;
            }
            return false;
        }

        [GeneratedRegex("^\\D+")]
        private static partial Regex GetPolicyNameRegex();

        [GeneratedRegex("[0-9]+$")]
        private static partial Regex GetServerIdRegex();
    }
    public enum ServerPolicies
    {
        SendMessages,
        ManageMessages,
        ServerMember
    }
}