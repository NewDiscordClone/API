using System.Text.RegularExpressions;

namespace Sparkle.Application.Common.RegularExpressions
{
    public static partial class Regexes
    {
        public static partial class Authorization
        {
            [GeneratedRegex("(?<=profileId:)(?i)(?![{(]?[0]{8}[-]?(?:[0]{4}[-]?){3}[0]{12}[)}]?)(?>([0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12})|{[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}}|[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}|[0-9A-F]{32})")]
            private static partial Regex GetProfileIdRegex();
            public static Regex ProfileIdRegex => GetProfileIdRegex();

            [GeneratedRegex("Roles:([\\w, -]+)")]
            private static partial Regex GetRolesRegex();
            public static Regex RolesRegex => GetRolesRegex();

            [GeneratedRegex("Claims:([\\w, -]+)")]
            private static partial Regex GetClaimsRegex();
            public static Regex ClaimsRegex => GetClaimsRegex();

            [GeneratedRegex("Policy:([^|]+)")]
            private static partial Regex GetPolicyRegex();
            public static Regex PolicyRegex => GetPolicyRegex();
        }
    }
}
