using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeRangePriority
{
    public record ChangeRangePriorityCommand(Dictionary<Guid, int> Priorities)
        : IRequest<IEnumerable<Role>>;
}
