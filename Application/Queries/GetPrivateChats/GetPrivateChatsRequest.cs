using Application.Models;
using Application.Models.LookUps;
using MediatR;

namespace Application.Queries.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChatLookUp>>
    {
        
    }
}