using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Queries.ServerRolesList
{
    public class ServerRolesListQueryHandler : RequestHandlerBase, IRequestHandler<ServerRolesListQuery, List<Role>>
    {
        public ServerRolesListQueryHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<List<Role>> Handle(ServerRolesListQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            List<Role> roles = await Context.SqlRoles.FilterAsync(role => role.ServerId == query.ServerId);
            return roles;
        }
    }
}
