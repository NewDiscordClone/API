using MediatR;

namespace Application.Commands.Servers.ChangeServerProfileRoles
{
    public record ChangeServerProfileRolesRequest : IRequest
    {
        public string ServerId { get; init; }
        public List<Guid> Roles { get; init; }
        public Guid UserId { get; init; }
    }
}