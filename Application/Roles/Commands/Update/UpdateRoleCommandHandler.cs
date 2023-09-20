using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.Update
{
    public class UpdateRoleCommandHandler : RequestHandlerBase, IRequestHandler<UpdateRoleCommand, Role>
    {
        public UpdateRoleCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task<Role> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Role role = await Context.SqlRoles.FindAsync(request.Id);

            await Context.RemoveClaimsFromRoleAsync(role);

            role.Name = request.Name;
            role.Color = request.Color;
            role.Priority = request.Priority;

            await Context.AddClaimsToRoleAsync(role, request.Claims);
            await Context.SaveChangesAsync();

            return role;
        }
    }
}
