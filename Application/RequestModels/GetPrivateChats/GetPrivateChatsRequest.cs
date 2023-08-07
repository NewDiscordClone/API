using Application.Models;
using MediatR;

namespace Application.RequestModels.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<GetPrivateChatDto>>
    {
        public int UserId { get; init; }
    }
}