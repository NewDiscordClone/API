using Application.Models;
using MediatR;

namespace Application.Queries.GetPersonalChats
{
    public record GetPrivateChatsRequest : IRequest<List<GetPrivateChatLookUpDto>>
    {
        
    }
}