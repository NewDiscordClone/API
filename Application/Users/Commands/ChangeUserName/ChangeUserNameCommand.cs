using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeUserNameCommand(string Username) : IRequest<User>
    {
    }
}
