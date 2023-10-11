using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Sparkle.WebApi.Authorization;
using Sparkle.WebApi.Authorization.Requirements;
using Sparkle.WebApi.Common.Parsers;
using static Sparkle.Application.Common.Constants.Constants;

namespace WebApi.Providers
{
    public class ServerAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private AuthorizationPolicyBuilder _policyBuilder;
        private IAuthorizationParser _parser;
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public ServerAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IAuthorizationParser parser)
        {
            FallbackPolicyProvider = new(options);
            _parser = parser;
        }

        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
           await FallbackPolicyProvider.GetDefaultPolicyAsync();

        public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
            await FallbackPolicyProvider.GetFallbackPolicyAsync();


        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policy)
        {
            if (!_parser.TryParseProfileId(policy, out Guid profileId))
                throw new InvalidOperationException("No profile id provided");
            _policyBuilder = new();

            if (_parser.TryParseName(policy, out string policyName))
                return await GetDefaultPolicyAsync(policyName, profileId);

            if (_parser.TryParseRoles(policy, out string[] roles))
            {
                _policyBuilder.RequireProfileRole(profileId, roles);
            }

            if (_parser.TryParseClaims(policy, out string[] claims))
            {
                _policyBuilder.RequireRoleClaims(profileId, claims);
            }

            return _policyBuilder.Build();

        }

        private async Task<AuthorizationPolicy?> GetDefaultPolicyAsync(string policyName, Guid profileId)
        {
            switch (policyName)
            {
                case Policies.SendMessages:
                    throw new NotImplementedException();// TODO: Добавьте логику для SendMessages
                case Policies.ChangeProfileName:

                    RoleClaimsRequirement claimRequirement = new(profileId,
                        new List<string> { Claims.ChangeSomeoneServerName });

                    ProfileOwnerRequirement profileRequirement = new(profileId);

                    _policyBuilder.AddRequirement(claimRequirement.Or(profileRequirement));
                    return _policyBuilder.Build();
                default:
                    return await FallbackPolicyProvider.GetPolicyAsync(policyName);
            };
        }
    }
}