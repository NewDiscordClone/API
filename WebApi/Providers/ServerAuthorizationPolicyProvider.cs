using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Sparkle.Application.Common;
using Sparkle.WebApi.Authorization;
using System.Text.RegularExpressions;

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

            if (!TryParseProfileId(policyName, out Guid profileId))
                throw new InvalidOperationException("No profile id provided");

            AuthorizationPolicyBuilder policy = new();

            switch (policyType)
            {
                case ServerPolicies.SendMessages:
                //  case ServerPolicies.ServerMember:
                //    policy.AddRequirements(new ServerMemberRequirement(profileId));
                //   return policy.Build();
                case ServerPolicies.ManageMessages:
                    policy.RequireRoleClaims(profileId, ServerClaims.ManageMessages);
                    return policy.Build();
                case ServerPolicies.MangeRoles:
                    policy.RequireRoleClaims(profileId,
                        ServerClaims.ManageRoles);
                    return policy.Build();
                case ServerPolicies.ManageServer:
                    policy.RequireRoleClaims(profileId,
                        ServerClaims.ManageServer);
                    return policy.Build();
                case ServerPolicies.ChangeName:
                    policy.RequireRoleClaims(profileId, ServerClaims.ChangeServerName);
                    return policy.Build();
                case ServerPolicies.ChangeSomeoneName:
                    policy.RequireRoleClaims(profileId, ServerClaims.ChangeSomeoneServerName);
                    return policy.Build();
                case ServerPolicies.RemoveMembers:
                    policy.RequireRoleClaims(profileId, ServerClaims.RemoveMembers);
                    return policy.Build();
                default:
                    return await FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
        }

        private bool TryParseProfileId(string policyName, out Guid profileId)
        {
            profileId = default;

            if (string.IsNullOrEmpty(policyName))
            {
                return false;
            }

            Regex serverIdRegex = GetProfileIdRegex();
            Match match = serverIdRegex.Match(policyName);
            if (match.Success)
            {
                profileId = Guid.Parse(match.Value);
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
            //TODO: check is policy is server policy
            Regex policyNameRegex = GetPolicyNameRegex();
            string name = policyNameRegex.Match(policyName).Value;
            if (Enum.TryParse(name, true, out policyType))
            {
                return true;
            }
            return false;
        }

        [GeneratedRegex("^\\w+")]
        private static partial Regex GetPolicyNameRegex();

        [GeneratedRegex("(?<=profileId:)(?i)(?![{(]?[0]{8}[-]?(?:[0]{4}[-]?){3}[0]{12}[)}]?)(?>([0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12})|{[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}}|[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}|[0-9A-F]{32})")]
        private static partial Regex GetProfileIdRegex();
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