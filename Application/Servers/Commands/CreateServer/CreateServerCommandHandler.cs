using MediatR;
using Sparkle.Application.Common.Factories;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandHandler : RequestHandler, IRequestHandler<CreateServerCommand, Server>
    {
        private readonly ServerFactory _serverFactory;
        private readonly IServerRepository _serverRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IChatRepository _chatRepository;

        public async Task<Server> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            ServerTemplates template = command.Template ?? ServerTemplates.Default;

            (Server server, ServerProfile owner, List<Channel> channels) =
                template switch
                {
                    ServerTemplates.Study => _serverFactory.StudyServer(command.Title, command.Image),
                    ServerTemplates.Gaming => _serverFactory.GamingServer(command.Title, command.Image),
                    ServerTemplates.School => _serverFactory.SchoolServer(command.Title, command.Image),
                    ServerTemplates.Friends => _serverFactory.FriendsServer(command.Title, command.Image),
                    ServerTemplates.Default or _ => _serverFactory.DefaultServer(command.Title, command.Image)
                };

            await _serverProfileRepository.AddAsync(owner, cancellationToken);
            await _serverRepository.AddAsync(server, cancellationToken);
            await _chatRepository.AddManyAsync(channels, cancellationToken);

            return server;
        }

        public CreateServerCommandHandler(
            IAuthorizedUserProvider userProvider,
            ServerFactory serverFactory,
            IServerProfileRepository serverProfileRepository,
            IServerRepository serverRepository,
            IChatRepository chatRepository)
            : base(userProvider)
        {
            _serverFactory = serverFactory;
            _serverProfileRepository = serverProfileRepository;
            _serverRepository = serverRepository;
            _chatRepository = chatRepository;
        }
    }
}