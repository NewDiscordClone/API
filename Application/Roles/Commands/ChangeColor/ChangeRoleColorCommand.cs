using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.ChangeColor
{
    public record ChangeRoleColorCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public string Color { get; init; }
    }
}
