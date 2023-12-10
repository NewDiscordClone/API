using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record UnblockUserCommand(Guid UserId) : IRequest<Relationship>
    {
    }
}
