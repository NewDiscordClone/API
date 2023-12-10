using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record BlockUserCommand(Guid UserId) : IRequest<Relationship>
    {
    }
}
