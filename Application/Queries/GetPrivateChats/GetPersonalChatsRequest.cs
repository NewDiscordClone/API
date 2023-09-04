using Application.Models;
using MediatR;

namespace Application.Queries.GetPersonalChats
{
    public record GetPersonalChatsRequest : IRequest<List<PrivateChatLookUp>>
    {
        
    }
}