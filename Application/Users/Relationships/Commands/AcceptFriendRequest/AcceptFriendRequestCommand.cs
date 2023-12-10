using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record AcceptFriendRequestCommand : IRequest<Relationship>
    {
        /// <summary>
        /// The unique identifier of the user who sent the friend request to accept it
        /// </summary>
        public Guid FriendId { get; init; }
    }
}