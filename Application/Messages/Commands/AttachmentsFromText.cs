using Sparkle.Application.Common.RegularExpressions;
using Sparkle.Domain.Messages.ValueObjects;
using System.Text.RegularExpressions;

namespace Sparkle.Application.Messages.Commands
{
    internal static partial class AttachmentsFromText
    {
        private static readonly Regex _urlRegEx = Regexes.UrlRegex;
        public static void GetAttachments(string input, Action<Attachment> onGet)
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