using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Commands.AddReaction
{
    public record AddReactionCommand : IRequest<Message>
    {
        /// <summary>
        /// Id of the message to add a reaction to
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Emoji code
        /// </summary>
        [DefaultValue(":smile:")]
        public string Emoji { get; init; }
    }

}