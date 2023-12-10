using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeColor
{
    public record ChangeRoleColorCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public string Color { get; init; }
    }
}
