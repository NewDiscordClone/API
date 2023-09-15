using MediatR;

namespace Sparkle.Application.Servers.Commands.ChangeServerProfileRoles
{
    public record UpdateServerProfileRolesCommand : IRequest
    {
        public string ServerId { get; init; }
        public List<Guid> Roles { get; init; }
        public Guid UserId { get; init; }
    }
}