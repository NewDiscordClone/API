using System.Text.RegularExpressions;
namespace Application.Common
{
    public static partial class AppRegexes
    {
        [GeneratedRegex(@"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\/api\/media\/[a-z0-9]{24}$")]
        public static partial Regex GetAvatarUrlRegex();

        [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")]
        public static partial Regex GetColorRegex();
    }
}
