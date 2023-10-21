using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatDetails
{
    public record PrivateChatDetailsQuery : IRequest<PrivateChatViewModel>
    {
        /// <summary>
        /// Id of the Group Chat
        /// </summary>
        public string ChatId { get; init; }
    }
}