using MediatR;

namespace Application.Commands.Users.FriendRequest
{
    public record FriendRequestRequest : IRequest<string?>
    {
        /// <summary>
        /// The unique identifier of the user for whom to send a friend request.
        /// </summary>
        public int UserId { get; init; }
    }
}
