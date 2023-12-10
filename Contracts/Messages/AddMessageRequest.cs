using Sparkle.Domain.Messages.ValueObjects;
using System.ComponentModel;

namespace Sparkle.Contracts.Messages
{
    public record AddMessageRequest
    {
        /// <summary>
        /// Text of the message. Can include links
        /// </summary>
        [DefaultValue("MessageText")]
        public string Text { get; init; }

        /// <summary>
        /// List of URL attachments that are not included in the message text
        /// </summary>
        public List<Attachment>? Attachments { get; init; }
    }
}
