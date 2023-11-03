using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record UnblockUserCommand(Guid UserId) : IRequest<Relationship>
    {
    }
}
