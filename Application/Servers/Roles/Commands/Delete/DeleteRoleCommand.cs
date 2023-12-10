using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public record DeleteRoleCommand : IRequest<Role>
    {
        public Guid RoleId { get; set; }
    }
}
