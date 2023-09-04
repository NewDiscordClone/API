using Application.Models;
using Application.Models.LookUps;
using MediatR;

namespace Application.Queries.GetPersonalChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChatLookUp>>
    {
        
    }
}