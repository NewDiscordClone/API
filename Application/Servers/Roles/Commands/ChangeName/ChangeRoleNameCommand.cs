using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeName
{
    public record ChangeRoleNameCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public string Name { get; init; }
    }
}
