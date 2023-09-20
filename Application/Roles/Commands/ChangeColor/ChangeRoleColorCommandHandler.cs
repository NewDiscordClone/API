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

        public async Task<Role> Handle(ChangeRoleColorCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(request.RoleId);
            role.Color = request.Color;

            await Context.SaveChangesAsync();
            return role;
        }
    }
}
