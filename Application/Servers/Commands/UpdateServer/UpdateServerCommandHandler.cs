using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.UpdateServer
{
    public class UpdateServerCommandHandler : RequestHandler, IRequestHandler<UpdateServerCommand, Server>
    {
        private readonly IServerRepository _serverRepository;

        public async Task<Server> Handle(UpdateServerCommand command, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(command.ServerId, cancellationToken);

            //TODO Remove image
            //if (command.Image != null && server.Image != null)
            //    await Context.CheckRemoveMedia(server.Image[(server.Image.LastIndexOf('/') - 1)..]);

            server.Title = command.Title ?? server.Title;
            server.Image = command.Image ?? server.Image;

            await _serverRepository.UpdateAsync(server, cancellationToken);
            return server;
        }

        public UpdateServerCommandHandler(IAuthorizedUserProvider userProvider,
            IServerRepository serverRepository) : base(userProvider)
        {
            _serverRepository = serverRepository;
        }
    }
}