using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.PrivateChatsList
{
    public record PrivateChatsQuery : IRequest<List<PrivateChatLookUp>>
    {

    }
}