using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record CancelFriendRequestCommand : IRequest<Relationship>
    {
        public Guid FriendId { get; init; }
    }
}
