using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Sparkle.WebApi.Authorization;
using System.Text.RegularExpressions;
using static Sparkle.Application.Common.Constants.Constants;

namespace WebApi.Providers
{
    public partial class ServerAuthorizationPolicyProvider : IAuthorizationPolicyProvider
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
            if (!TryParsePolicyType(policy, out string policyName))
                return await FallbackPolicyProvider.GetPolicyAsync(policyName);

            if (!TryParseProfileId(policy, out Guid profileId))
                throw new InvalidOperationException("No profile id provided");

            _policyBuilder = new();

            return policyName switch
            {
                Policies.ManageMessages => GetClaimsPolicy(profileId, Claims.ManageMessages),
                Policies.SendMessages => throw new NotImplementedException(),// TODO: Добавьте логику для SendMessages, если необходимо
                Policies.ManageRoles => GetClaimsPolicy(profileId, Claims.ManageRoles),
                Policies.ManageServer => GetClaimsPolicy(profileId, Claims.ManageServer),
                Policies.ChangeName => GetClaimsPolicy(profileId, Claims.ChangeServerName),
                Policies.ChangeSomeoneName => GetClaimsPolicy(profileId, Claims.ChangeSomeoneServerName),
                Policies.RemoveMembers => GetClaimsPolicy(profileId, Claims.RemoveMembers),
                Policies.DeleteServer => GetRolesPolicy(profileId, Roles.ServerOwnerName),
                Policies.CreateInvitation => GetClaimsPolicy(profileId, Claims.CreateInvitation), //TODO Добавить возможность проверить настройки сервера
                _ => await FallbackPolicyProvider.GetPolicyAsync(policyName),
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

        private static bool TryParsePolicyType(string policy, out string policyName)
        {
            policyName = string.Empty;

            if (string.IsNullOrEmpty(policy))
                return false;

            Regex policyNameRegex = GetPolicyNameRegex();
            string name = policyNameRegex.Match(policy).Value;

            if (Policies.GetPolicies().Contains(name))
            {
                policyName = name;
                return true;
            }
            return false;
        }

        [GeneratedRegex("^\\w+")]
        private static partial Regex GetPolicyNameRegex();

        [GeneratedRegex("(?<=profileId:)(?i)(?![{(]?[0]{8}[-]?(?:[0]{4}[-]?){3}[0]{12}[)}]?)(?>([0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12})|{[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}}|[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}|[0-9A-F]{32})")]
        private static partial Regex GetProfileIdRegex();
    }
}