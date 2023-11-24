using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Commands.RemoveAllReactions
{
    public record RemoveAllReactionsCommand : IRequest<Message>
    {
        /// <summary>
        /// Id of the message for which all reactions should be removed
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}