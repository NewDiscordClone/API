using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeRangePriority
{
    public record ChangeRangePriorityCommand(Dictionary<Guid, int> Priorities)
        : IRequest<IEnumerable<Role>>;
}
