using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public record RemoveAttachmentCommand : IRequest<Message>
    {
        /// <summary>
        /// Id of the message to which the attachment is attached
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Index of the attachment in the message's attachments collection 
        /// </summary>
        [DefaultValue(0)]
        public int AttachmentIndex { get; init; }
    }
}