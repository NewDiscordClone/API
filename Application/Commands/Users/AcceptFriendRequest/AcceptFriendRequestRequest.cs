using MediatR;

namespace Application.Commands.Users.AcceptFriendRequest
{
    public record AcceptFriendRequestRequest : IRequest
    {
        public int UserId { get; init; }
    }
}