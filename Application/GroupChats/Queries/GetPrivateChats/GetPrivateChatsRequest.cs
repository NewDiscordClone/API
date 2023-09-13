using Application.Models.LookUps;
using MediatR;

namespace Application.GroupChats.Queries.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChatLookUp>>
    {

    }
}