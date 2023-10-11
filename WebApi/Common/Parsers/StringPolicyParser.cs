using Sparkle.Application.Common.RegularExpressions;
using System.Text.RegularExpressions;
using static Sparkle.Application.Common.Constants.Constants;

namespace Sparkle.WebApi.Common.Parsers
{
    public class StringPolicyParser : IAuthorizationParser
    {

        public bool TryParseClaims(string policy, out string[] claims)
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

        public bool TryParseRoles(string policy, out string[] roles)
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

        public bool TryParseName(string policy, out string policyName)
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

        public bool TryParseProfileId(string policyName, out Guid profileId)
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

        public string ParseName(string policy)
        {
            return TryParseName(policy, out string policyName) ? policyName
                : throw new InvalidOperationException("No policy name provided");
        }

        public Guid ParseProfileId(string policy)
        {
            return TryParseProfileId(policy, out Guid id) ? id
                : throw new InvalidOperationException("No profile id provided");
        }

        public string[] ParseRoles(string policy)
        {
            return TryParseRoles(policy, out string[] roles) ? roles
                : throw new InvalidOperationException("No roles provided");
        }

        public string[] ParseClaims(string policy)
        {
            return TryParseClaims(policy, out string[] claims) ? claims
                : throw new InvalidOperationException("No claims provided");
        }
    }
}
