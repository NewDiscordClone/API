using MediatR;
using Sparkle.Domain.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatsList
{
    public record PrivateChatsQuery : IRequest<List<PrivateChatLookUp>>
    {

    }
}