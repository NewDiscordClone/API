using MediatR;
using WebApi.Models;

namespace Application.RequestModels.PrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<PrivateChat>>
    {
        public int UserID { get; init; }
    }
}