namespace Sparkle.WebApi.Common.Parsers
{
    public interface IAuthorizationParser
    {
        string ParseName(string policy);
        bool TryParseName(string policy, out string name);

        Guid ParseProfileId(string policy);
        bool TryParseProfileId(string policy, out Guid profileId);

        string[] ParseRoles(string policy);
        bool TryParseRoles(string policy, out string[] roles);

        string[] ParseClaims(string policy);
        bool TryParseClaims(string policy, out string[] claims);
    }
}
