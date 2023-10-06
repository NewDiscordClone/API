using MediatR;
using Sparkle.Application.Chats.Queries.PrivateChatDetails;

namespace Sparkle.Application.Chats.PersonalChats.Queries
{
    public record GetPersonalChatByUserIdQuery : IRequest<PrivateChatViewModel>
    {
        public Guid UserId { get; init; }
    }
}
