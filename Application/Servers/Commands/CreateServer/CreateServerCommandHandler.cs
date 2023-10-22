using MediatR;
using Sparkle.Application.Common.Factories;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandHandler : RequestHandlerBase, IRequestHandler<CreateServerCommand, Server>
    {
        private readonly ServerFactory _serverFactory;
        private readonly IServerProfileRepository _serverProfileRepository;
        public async Task<Server> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            ServerType template = command.Template ?? ServerType.Default;

            (Server server, ServerProfile owner, List<Channel> channels) =
                template switch
                {
                    ServerType.Study => _serverFactory.StudyServer(command.Title, command.Image),
                    ServerType.Gaming => _serverFactory.GamingServer(command.Title, command.Image),
                    ServerType.Default or _ => _serverFactory.DefaultServer(command.Title, command.Image)
                };

            await _serverProfileRepository.AddAsync(owner, cancellationToken);
            await Context.Servers.AddAsync(server, cancellationToken);
            await Context.Channels.AddManyAsync(channels, cancellationToken);

            return server;
        }

        public CreateServerCommandHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            ServerFactory serverFactory,
            IServerProfileRepository serverProfileRepository)
            : base(context, userProvider)
        {
            _serverFactory = serverFactory;
            _serverProfileRepository = serverProfileRepository;
        }
    }
}