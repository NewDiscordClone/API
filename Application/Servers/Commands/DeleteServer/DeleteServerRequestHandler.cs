using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Commands.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerCommand, (Server Server, IEnumerable<Guid> UserIds)>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IServerProfileRepository _serverProfileRepository;

        public IAppDbContext GetContext()
        {
            return Context;
        }

        public async Task<(Server Server, IEnumerable<Guid> UserIds)> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(request.ServerId, cancellationToken);
            Guid[] userIds = await Context.UserProfiles
                .OfType<ServerProfile>()
                .Where(profile => profile.ServerId == server.Id)
                .Select(profile => profile.UserId)
                .ToArrayAsync();


            await _roleRepository.DeleteManyAsync(role => role.ServerId == request.ServerId, cancellationToken);
            await _serverProfileRepository.DeleteManyAsync(profile => profile.ServerId == request.ServerId, cancellationToken);
            await Context.Servers.DeleteAsync(server, cancellationToken);
            await Context.Channels.DeleteManyAsync(c => c.ServerId == request.ServerId, cancellationToken);

            return (server, userIds);
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IRoleRepository roleRepository, IServerProfileRepository serverProfileRepository) : base(context, userProvider)
        {
            _roleRepository = roleRepository;
            _serverProfileRepository = serverProfileRepository;
        }
    }
}
