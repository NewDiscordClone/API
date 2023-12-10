using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeUserNameCommand(string Username) : IRequest<User>
    {
    }
}
