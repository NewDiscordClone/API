using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public record DeleteRoleCommand : IRequest<Role>
    {
        public Guid RoleId { get; set; }
    }
}
