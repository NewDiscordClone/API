using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.GroupChats.Queries.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChatLookUp>>
    {

    }
}