using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.ChangeColor
{
    public class ChangeRoleColorCommandHandler : RequestHandlerBase, IRequestHandler<ChangeRoleColorCommand, Role>
    {
        public ChangeRoleColorCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(ChangeRoleColorCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(command.RoleId);
            role.Color = command.Color;

            await Context.SaveChangesAsync();
            return role;
        }
    }
}
