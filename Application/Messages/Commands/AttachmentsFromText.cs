using Sparkle.Application.Common.RegularExpressions;
using Sparkle.Application.Models;
using System.Text.RegularExpressions;

namespace Sparkle.Application.Messages.Commands
{
    internal static class AttachmentsFromText
    {
        private static readonly Regex _urlRegEx = Regexes.UrlRegex;
        public static void GetAttachments(this string input, Action<Attachment> onGet)
        {
            MatchCollection urlCollection = _urlRegEx.Matches(input);
            foreach (Match match in urlCollection)
            {
                onGet(new Attachment
                {
                    IsInText = true,
                    Path = match.Value,
                    IsSpoiler = false
                });
            }
        }


    }
}