using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangeName
{
    public class ChangeRoleNameCommandHandler : RequestHandlerBase, IRequestHandler<ChangeRoleNameCommand, Role>
    {
        public ChangeRoleNameCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(ChangeRoleNameCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(command.RoleId);
            role.Name = command.Name;

            await Context.SqlRoles.UpdateAsync(role);
            return role;
        }
    }
}
