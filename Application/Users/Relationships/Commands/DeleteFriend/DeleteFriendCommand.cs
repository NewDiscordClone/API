using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record DeleteFriendCommand : IRequest<Relationship>
    {
        public Guid FriendId { get; init; }
    }
}
