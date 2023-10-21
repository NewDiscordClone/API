using System.Text.RegularExpressions;
namespace Sparkle.Application.Common.RegularExpressions
{
    public static partial class Regexes
    {
        [GeneratedRegex("^https?://(www.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}/api/media/[a-z0-9]{24}\\.[a-zA-Z0-9]+$")]
        private static partial Regex GetMediaUrlRegex();

        public static Regex MediaUrlRegex => GetMediaUrlRegex();

        [GeneratedRegex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$")]
        private static partial Regex GetColorRegex();
        public static Regex ColorRegex => GetColorRegex();

        [GeneratedRegex("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)")]
        private static partial Regex GetUrlRegex();
        public static Regex UrlRegex => GetUrlRegex();

        [GeneratedRegex(":\\w+:")]
        private static partial Regex GetEmojiRegex();
        public static Regex EmojiRegex => GetEmojiRegex();

        [GeneratedRegex("[a-z0-9]{24}")]
        private static partial Regex GetObjectIdRegex();
        public static Regex ObjectIdRegex => GetObjectIdRegex();

        [GeneratedRegex("^[0-9a-z_.]*$")]
        private static partial Regex GetUserNameRegex();
        public static Regex UserNameRegex => GetUserNameRegex();
    }
}
