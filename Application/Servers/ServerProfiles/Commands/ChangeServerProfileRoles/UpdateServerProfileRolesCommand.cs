using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileRoles
{
    public record UpdateServerProfileRolesCommand : IRequest<ServerProfile>
    {
        public List<Guid> Roles { get; init; }
        public Guid ProfileId { get; init; }
    }
}