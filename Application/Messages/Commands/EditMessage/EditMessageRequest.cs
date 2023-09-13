using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Messages.Commands.EditMessage
{
    public record EditMessageRequest : IRequest
    {
        /// <summary>
        /// Id of the message to edit
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// New message text. May include links
        /// </summary>
        [Required]
        [MaxLength(2000)]
        [MinLength(1)]
        [DefaultValue("NewTextString")]
        public string NewText { get; init; }
    }

}