using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Commands
{
    public record BlockUserCommand(Guid UserId) : IRequest<Relationship>
    {
    }
}
