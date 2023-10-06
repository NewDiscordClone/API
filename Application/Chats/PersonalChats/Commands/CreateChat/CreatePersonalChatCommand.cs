using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.PersonalChats.Commands.CreateChat
{
    public record CreatePersonalChatCommand : IRequest<PersonalChat>
    {
        public Guid UserId { get; init; }
    }
}
