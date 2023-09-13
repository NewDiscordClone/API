using MediatR;

namespace Application.Users.Commands.AcceptFriendRequest
{
    public record AcceptFriendRequestRequest : IRequest
    {
        /// <summary>
        /// The unique identifier of the user who sent the friend request to accept it
        /// </summary>
        public Guid UserId { get; init; }
    }
}