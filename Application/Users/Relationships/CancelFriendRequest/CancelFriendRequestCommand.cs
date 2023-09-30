using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.CancelFriendRequest
{
    public record CancelFriendRequestCommand : IRequest<Relationship>
    {
        public Guid FriendId { get; init; }
    }
}
