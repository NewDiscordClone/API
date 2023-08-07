using Application.Models;
using MediatR;

namespace Application.RequestModels.PrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChat>>
    {
        public int UserID { get; init; }
    }
}