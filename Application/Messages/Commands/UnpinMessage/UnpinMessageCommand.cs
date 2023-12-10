using MediatR;
using Sparkle.Domain.LookUps;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Commands.UnpinMessage
{
    public record UnpinMessageCommand : IRequest<MessageDto>
    {
        /// <summary>
        /// Id of message to be unpinned
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}