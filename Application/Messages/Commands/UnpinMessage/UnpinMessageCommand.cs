using MediatR;
using Sparkle.Application.Models.LookUps;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Messages.Commands.UnpinMessage
{
    public record UnpinMessageCommand : IRequest<MessageDto>
    {
        /// <summary>
        /// Id of message to be unpinned
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}