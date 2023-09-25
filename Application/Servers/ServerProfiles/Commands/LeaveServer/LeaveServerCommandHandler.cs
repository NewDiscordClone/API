using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.LeaveServer
{
    public class LeaveServerCommandHandler : RequestHandlerBase, IRequestHandler<LeaveServerCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public LeaveServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IServerProfileRepository serverProfileRepository) : base(context, userProvider)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(LeaveServerCommand command, CancellationToken cancellationToken)
        {
            Server server = await Context.Servers.FindAsync(command.ServerId);

            ServerProfile serverProfile = await _serverProfileRepository
                .SingleAsync(profile => profile.Id == command.ProfileId
                && profile.ServerId == command.ServerId);

            await _serverProfileRepository.DeleteAsync(serverProfile);

            server.Profiles.Remove(serverProfile.Id);
            await Context.Servers.UpdateAsync(server);
        }
    }
}