using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public record ChangeAvatarCommand(string AvatarUrl) : IRequest<User>;


}
