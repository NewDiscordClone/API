using System.Text.RegularExpressions;
namespace Sparkle.Application.Common.RegularExpressions
{
    public static partial class Regexes
    {
        [GeneratedRegex("^https?://(www.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}/api/media/[a-z0-9]{24}$")]
        public static partial Regex AvatarUrlRegex();

        [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")]
        public static partial Regex ColorRegex();

        [GeneratedRegex(":\\w+:")]
        public static partial Regex EmojiRegex();

        [GeneratedRegex("[a-z0-9]{24}")]
        public static partial Regex ObjectIdRegex();
    }
}
