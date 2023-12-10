using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Chats.PersonalChats.Commands.CreateChat
{
    public record CreatePersonalChatCommand : IRequest<PersonalChat>
    {
        public Guid UserId { get; init; }
    }
}
