using MediatR;

namespace Application.Queries.GetPrivateChats
{
    public record GetPrivateChatsRequest : IRequest<List<GetPrivateChatLookUpDto>>
    {
        public int UserId { get; init; }
    }
}