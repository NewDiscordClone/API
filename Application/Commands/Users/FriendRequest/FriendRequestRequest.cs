using MediatR;

namespace Application.Commands.Users.FriendRequest
{
    public record FriendRequestRequest : IRequest<string?>
    {
        public int UserId { get; init; }
    }
}