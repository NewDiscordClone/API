using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionCommand : IRequest
    {
        /// <summary>
        /// Id of the message to which the reaction is attached
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Emoji
        /// </summary>
        [DefaultValue("😀")]
        public string Emoji { get; init; }
    }
}