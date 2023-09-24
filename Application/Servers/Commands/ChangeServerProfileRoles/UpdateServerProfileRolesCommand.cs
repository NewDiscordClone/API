using MediatR;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileRoles
{
    public record UpdateServerProfileRolesCommand : IRequest
    {
        public List<Guid> Roles { get; init; }
        public Guid ProfileId { get; init; }
    }
}