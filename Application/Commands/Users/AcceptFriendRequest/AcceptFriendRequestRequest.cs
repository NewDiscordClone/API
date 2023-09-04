using MediatR;

namespace Application.Commands.Users.AcceptFriendRequest
{
    public record AcceptFriendRequestRequest : IRequest
    {
        public Guid UserId { get; init; }
    }
}