using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.ChangePriority
{
    public class ChangeRolePriorityCommandHandler : RequestHandlerBase, IRequestHandler<ChangeRolePriorityCommand, Role>
    {
        public ChangeRolePriorityCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(ChangeRolePriorityCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(request.RoleId);
            role.Priority = request.Priority;

            await Context.SaveChangesAsync();
            return role;
        }
    }
}
