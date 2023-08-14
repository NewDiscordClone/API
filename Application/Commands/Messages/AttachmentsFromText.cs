using System.Text.RegularExpressions;
using Application.Models;

namespace Application.Commands.Messages
{
    internal static class AttachmentsFromText
    {
        private static readonly Regex _urlRegEx =
            new Regex(
                @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/=]*)");

        private static readonly Regex _imageUrlRegEx =
            new Regex(
                @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#\/=]*)\.(?:png|jpg)([-a-zA-Z0-9()@:%_\+.~#&?\/=]*)");

        private static readonly Regex _gifUrlRegEx =
            new Regex(
                @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#\/=]*)\.(?:gif)([-a-zA-Z0-9()@:%_\+.~#&?\/=]*)");

        public static void GetAttachments(string input, Action<Attachment> onGet)
        {
            var urlCollection = _urlRegEx.Matches(input);
            foreach (Match match in urlCollection)
            {
                var type = AttachmentType.Url;
                if (_imageUrlRegEx.IsMatch(match.Value))
                    type = AttachmentType.UrlImage;
                else if (_gifUrlRegEx.IsMatch(match.Value))
                    type = AttachmentType.UrlGif;
                
                onGet(new Attachment
                {
                    Path = match.Value,
                    Type = type,
                    IsSpoiler = false
                });
            }
        }
    }
}