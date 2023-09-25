using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Update
{
    public class UpdateRoleCommandHandler : RequestHandlerBase, IRequestHandler<UpdateRoleCommand, Role>
    {
        public UpdateRoleCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Role role = await Context.SqlRoles.FindAsync(command.Id);

            await Context.RemoveClaimsFromRoleAsync(role);

            role.Name = command.Name;
            role.Color = command.Color;
            role.Priority = command.Priority;

            await Context.AddClaimsToRoleAsync(role, command.Claims);
            await Context.SqlRoles.UpdateAsync(role);

            return role;
        }
    }
}
