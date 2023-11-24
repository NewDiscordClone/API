using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerCommand, (Server Server, IEnumerable<Guid> UserIds)>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IServerRepository _serverRepository;
        private readonly IChatRepository _chatRepository;

        public async Task<(Server Server, IEnumerable<Guid> UserIds)> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(request.ServerId, cancellationToken);

            Guid[] userIds = await _serverProfileRepository.ExecuteCustomQuery(profiles => profiles
                .Where(profile => profile.ServerId == server.Id)
                .Select(profile => profile.UserId))
                .ToArrayAsync(cancellationToken);


            await _roleRepository.DeleteManyAsync(role => role.ServerId == request.ServerId, cancellationToken);
            await _serverProfileRepository.DeleteManyAsync(profile => profile.ServerId == request.ServerId, cancellationToken);
            await _serverRepository.DeleteAsync(server, cancellationToken);

            IQueryable<Channel> channelsToDelete = _chatRepository.Channels
                .Where(channel => channel.ServerId == request.ServerId);
            await _chatRepository.DeleteManyAsync(channelsToDelete, cancellationToken);

            return (server, userIds);
        }

        public DeleteServerRequestHandler(IAuthorizedUserProvider userProvider,
            IRoleRepository roleRepository,
            IServerProfileRepository serverProfileRepository,
            IServerRepository serverRepository,
            IChatRepository chatRepository) : base(userProvider)
        {
            _roleRepository = roleRepository;
            _serverProfileRepository = serverProfileRepository;
            _serverRepository = serverRepository;
            _chatRepository = chatRepository;
        }
    }
}
