using MediatR;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public record DeleteRoleCommand : IRequest
    {
        public Guid RoleId { get; set; }
    }
}
