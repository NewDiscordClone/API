using System.Text.RegularExpressions;

namespace Sparkle.Application.Common.RegularExpressions
{
    public static partial class Regexes
    {
        public static partial class Password
        {
            [GeneratedRegex("[A-Z]")]
            public static partial Regex HasUpperCase();

            [GeneratedRegex("[a-z]")]
            public static partial Regex HasLowerCase();

            [GeneratedRegex("[0-9]")]
            public static partial Regex HasDigit();

            [GeneratedRegex("[^a-zA-Z0-9]")]
            public static partial Regex HasSpecialCharacter();
        }
    }
}
