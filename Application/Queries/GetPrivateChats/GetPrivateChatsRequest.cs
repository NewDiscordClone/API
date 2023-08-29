using Application.Models;
using MediatR;

namespace Application.Queries.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChat>>
    {
        
    }
}