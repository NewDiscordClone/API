using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.ChangeName
{
    public class ChangeRoleNameCommandHandler : RequestHandlerBase, IRequestHandler<ChangeRoleNameCommand, Role>
    {
        public ChangeRoleNameCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(ChangeRoleNameCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(request.RoleId);
            role.Name = request.Name;

            await Context.SaveChangesAsync();
            return role;
        }
    }
}
