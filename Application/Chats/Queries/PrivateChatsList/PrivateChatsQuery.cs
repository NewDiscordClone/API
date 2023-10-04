using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Chats.Queries.PrivateChatsList
{
    public record PrivateChatsQuery : IRequest<List<PrivateChatLookUp>>
    {

    }
}