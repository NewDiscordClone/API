using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerCommand, Server>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        public async Task<Server> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(request.ServerId);

            await Context.Servers.DeleteAsync(server);
            await _roleRepository.DeleteManyAsync(role => role.ServerId == request.ServerId);
            await _serverProfileRepository.DeleteManyAsync(profile => profile.ServerId == request.ServerId);
            await Context.Channels.DeleteManyAsync(c => c.ServerId == request.ServerId);

            return server;
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IRoleRepository roleRepository) : base(context, userProvider)
        {
            _roleRepository = roleRepository;
        }
    }
}
