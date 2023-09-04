using MediatR;

namespace Application.Commands.Users.FriendRequest
{
    public record FriendRequestRequest : IRequest<string?>
    {
        /// <summary>
        /// The unique identifier of the user to send a friend request to.
        /// </summary>
        public int UserId { get; init; }
    }
}
