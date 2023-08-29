using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using WebApi.Authorization;
using WebApi.Authorization.Requirements;

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

            if (!TryParseServerId(policyName, out string serverId))
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
                case ServerPolicies.MangeRoles:
                    policy.RequireServerClaim(serverId,
                        ServerClaims.ManageRoles);
                    return policy.Build();
                case ServerPolicies.ManageServer:
                    policy.RequireServerClaim(serverId,
                        ServerClaims.ManageRoles);
                    return policy.Build();
                case ServerPolicies.ChangeName:
                    policy.RequireServerClaim(serverId, ServerClaims.ChangeServerName);
                    return policy.Build();
                case ServerPolicies.ChangeSomeoneName:
                    policy.RequireServerClaim(serverId, ServerClaims.ChangeSomeoneServerName);
                    return policy.Build();
                case ServerPolicies.RemoveMembers:
                    policy.RequireServerClaim(serverId, ServerClaims.RemoveMembers);
                    return policy.Build();
                default:
                    return await FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
        }

        private bool TryParseServerId(string policyName, out string serverId)
        {
            serverId = string.Empty;

            if (string.IsNullOrEmpty(policyName))
            {
                return false;
            }

            Regex serverIdRegex = GetServerIdRegex();
            Match match = serverIdRegex.Match(policyName);
            if (match.Success)
            {
                serverId = match.Value;
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
        ServerMember,
        ManageMessages,
        MangeRoles,
        ManageServer,
        ChangeName,
        ChangeSomeoneName,
        RemoveMembers,
    }
}