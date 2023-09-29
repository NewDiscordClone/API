using MediatR;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileRoles
{
    public record UpdateServerProfileRolesCommand : IRequest
    {
        public List<Guid> Roles { get; init; }
        public Guid ProfileId { get; init; }
    }
}