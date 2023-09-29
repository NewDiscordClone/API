using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Delete
{
    public class DeleteRoleCommandHandler : RequestHandlerBase, IRequestHandler<DeleteRoleCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public DeleteRoleCommandHandler(IAppDbContext context, IServerProfileRepository serverProfileRepository) : base(context)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Role role = await Context.SqlRoles.FindAsync(command.RoleId);

            if (role.ServerId is null)
                throw new InvalidOperationException("Role is not associated with a server");

            Server server = await Context.Servers.FindAsync(role.ServerId);

            server.Roles.Remove(role.Id);
            await _serverProfileRepository.RemoveRoleFromServerProfilesAsync(role, server.Id, cancellationToken);
            Context.Roles.Remove(role);

            await Context.SaveChangesAsync();
            await Context.Servers.UpdateAsync(server);
        }
    }
}
