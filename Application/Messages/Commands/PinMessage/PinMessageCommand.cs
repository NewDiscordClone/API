using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Commands.PinMessage
{
    public record PinMessageCommand : IRequest
    {
        /// <summary>
        /// Id of the message to be pinned
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}