using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeName
{
    public record ChangeRoleNameCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public string Name { get; init; }
    }
}
