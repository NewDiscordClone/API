﻿using Sparkle.Application.Models;
using System.Text.RegularExpressions;

namespace Sparkle.Application.Messages.Commands
{
    internal static class AttachmentsFromText
    {
        private static readonly Regex _urlRegEx =
            new(
                @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&\/=]*)");
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