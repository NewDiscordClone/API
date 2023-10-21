using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeTextStatusCommand(string? TextStatus) : IRequest<User>
    {
    }
}
