using System.Text.RegularExpressions;
namespace Application.Common
{
    public static partial class AppRegexes
    {
        [GeneratedRegex("^\\wt+")]
        public static partial Regex GetAvatarUrlRegex();

        [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")]
        public static partial Regex GetColorRegex();
    }
}
