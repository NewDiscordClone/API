using MediatR;

namespace Application.Commands.Servers.ChangeServerProfileRoles
{
    public record ChangeServerProfileRolesRequest : IRequest
    {
        public string ServerId { get; init; }
        public List<int> Roles { get; init; }
        public int UserId { get; init; }
    }
}