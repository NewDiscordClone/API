using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public record AddReactionRequest : IRequest
    {
        /// <summary>
        /// Id of the message to add a reaction to
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Emoji code
        /// </summary>
        [Required]
        [DefaultValue(":smile:")]
        public string Emoji { get; init; }
    }

}