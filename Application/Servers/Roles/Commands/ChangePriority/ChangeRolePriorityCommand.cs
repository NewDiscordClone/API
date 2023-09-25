using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangePriority
{
    public record ChangeRolePriorityCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public int Priority { get; init; }
    }
}
