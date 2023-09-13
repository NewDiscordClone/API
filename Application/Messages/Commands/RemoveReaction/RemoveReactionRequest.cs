using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionRequest : IRequest
    {
        /// <summary>
        /// Id of the message to which the reaction is attached
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Index of the reaction in the message's reactions collection
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int ReactionIndex { get; init; }
    }
}