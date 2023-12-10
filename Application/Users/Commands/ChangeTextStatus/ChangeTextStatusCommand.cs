using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeTextStatusCommand(string? TextStatus) : IRequest<User>
    {
    }
}
