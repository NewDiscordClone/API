using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.GetPrivateChats
{
    public record GetPrivateChatsQuery : IRequest<List<PrivateChatLookUp>>
    {

    }
}