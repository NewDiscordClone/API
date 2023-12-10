using MediatR;
using Sparkle.Domain.LookUps;
using System.ComponentModel;

namespace Sparkle.Application.Messages.Queries.GetMessages
{
    public record GetMessagesQuery : IRequest<List<MessageDto>>
    {
        /// <summary>
        /// The unique identifier of the chat from which to retrieve messages.
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The number of messages that have already been loaded and should be skipped.
        /// </summary>
        [DefaultValue(0)]
        public int MessagesCount { get; init; }
    }
}
