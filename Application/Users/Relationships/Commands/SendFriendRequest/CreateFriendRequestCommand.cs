using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record CreateFriendRequestCommand : IRequest<Relationship>
    {
        /// <summary>
        /// The unique identifier of the user to send a friend request to.
        /// </summary>
        public Guid FriendId { get; init; }
    }
}
