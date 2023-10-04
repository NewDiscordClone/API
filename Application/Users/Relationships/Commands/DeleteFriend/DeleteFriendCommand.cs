using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands.DeleteFriend
{
    public record DeleteFriendCommand : IRequest<Relationship>
    {
        public Guid FriendId { get; init; }
    }
}
