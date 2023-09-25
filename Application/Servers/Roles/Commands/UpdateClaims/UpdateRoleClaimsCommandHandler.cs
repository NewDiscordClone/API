using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.UpdateClaims
{
    public class UpdateRoleClaimsCommandHandler : RequestHandlerBase, IRequestHandler<UpdateRoleClaimsCommand, Role>
    {
        public UpdateRoleClaimsCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(UpdateRoleClaimsCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(command.RoleId);

            await Context.RemoveClaimsFromRoleAsync(role);
            await Context.AddClaimsToRoleAsync(role, command.Claims);
            await Context.SqlRoles.UpdateAsync(role);

            return role;
        }
    }
}
