using Application.Models;
using MediatR;

namespace Application.RequestModels.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<GetPrivateChatLookUpDto>>
    {
        public int UserId { get; init; }
    }
}