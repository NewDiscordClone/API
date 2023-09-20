using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.Delete
{
    public class DeleteRoleCommandHandler : RequestHandlerBase, IRequestHandler<DeleteRoleCommand>
    {
        public DeleteRoleCommandHandler(IAppDbContext context) : base(context)
        {
        }

        public async Task Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(command.RoleId);

            await Context.SqlRoles.DeleteAsync(command.RoleId);

            Server server = await Context.Servers.FindAsync(role.ServerId);

            server.Roles.Remove(role.Id);

            List<ServerProfile> profilesWithRole = server.ServerProfiles
                .Where(serverProfile => serverProfile.Roles.Contains(role))
                .ToList();

            profilesWithRole.ForEach(serverProfile => serverProfile.Roles.Remove(role));

            await Context.SaveChangesAsync();
        }
    }
}
