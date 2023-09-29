using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.AcceptFriendRequest
{
    public record AcceptFriendRequestCommand : IRequest<Relationship>
    {
        /// <summary>
        /// The unique identifier of the user who sent the friend request to accept it
        /// </summary>
        public Guid FriendId { get; init; }
    }
}