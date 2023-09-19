using MediatR;
using Sparkle.Application.Models.LookUps;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Queries.GetPinnedMessages
{
    public record GetPinnedMessagesQuery : IRequest<List<MessageDto>>
    {
        /// <summary>
        /// The unique identifier of the chat retrieve pinned messages from.
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
    }
}
