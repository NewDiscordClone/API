using MediatR;

namespace Application.Commands.Users.AcceptFriendRequest
{
    public record AcceptFriendRequestRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the user which friend request current user want to accept.
        /// </summary>
        public int UserId { get; init; }
    }
}
