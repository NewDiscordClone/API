using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.PersonalChats.Queries
{
    public record GetPersonalChatByUserIdQuery : IRequest<PrivateChatViewModel>
    {
        public Guid UserId { get; init; }
    }
}
