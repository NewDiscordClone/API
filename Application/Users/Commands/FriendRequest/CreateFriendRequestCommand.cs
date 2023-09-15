using MediatR;

namespace Sparkle.Application.Users.Commands.FriendRequest
{
    public record CreateFriendRequestCommand : IRequest<string?>
    {
        /// <summary>
        /// The unique identifier of the user to send a friend request to.
        /// </summary>
        public Guid UserId { get; init; }
    }
}
