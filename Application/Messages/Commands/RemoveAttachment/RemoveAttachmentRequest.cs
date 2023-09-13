using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Messages.Commands.RemoveAttachment
{
    public record RemoveAttachmentRequest : IRequest
    {
        /// <summary>
        /// Id of the message to which the attachment is attached
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Index of the attachment in the message's attachments collection 
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int AttachmentIndex { get; init; }
    }
}