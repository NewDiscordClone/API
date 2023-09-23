using MediatR;

namespace Sparkle.Application.Roles.Commands.Delete
{
    public record DeleteRoleCommand : IRequest
    {
        public Guid RoleId { get; set; }
    }
}
