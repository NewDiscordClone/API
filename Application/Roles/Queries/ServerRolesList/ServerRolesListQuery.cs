using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Queries.ServerRolesList
{
    public record ServerRolesListQuery : IRequest<List<Role>>
    {
        public string ServerId { get; init; }
    }
}
