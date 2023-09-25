using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.ChangePriority
{
    public class ChangeRolePriorityCommandHandler : RequestHandlerBase, IRequestHandler<ChangeRolePriorityCommand, Role>
    {
        public ChangeRolePriorityCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(ChangeRolePriorityCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(command.RoleId);
            role.Priority = command.Priority;

            await Context.SqlRoles.UpdateAsync(role);
            return role;
        }
    }
}
