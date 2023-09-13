using MediatR;

namespace Sparkle.Application.Common.Servers.Commands.ChangeServerProfileRoles
{
    public record ChangeServerProfileRolesRequest : IRequest
    {
        public string ServerId { get; init; }
        public List<Guid> Roles { get; init; }
        public Guid UserId { get; init; }
    }
}