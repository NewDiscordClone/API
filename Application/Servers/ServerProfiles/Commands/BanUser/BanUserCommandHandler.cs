using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.BanUser
{
    public class BanUserCommandHandler : RequestHandlerBase, IRequestHandler<BanUserCommand>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public BanUserCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IServerProfileRepository serverProfileRepository) : base(context, userProvider)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task Handle(BanUserCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(command.ServerId);

            if (!server.Profiles.Contains(command.ProfileId))
                return;


            ServerProfile profileToRemove = await _serverProfileRepository.FindAsync(command.ProfileId);
            await _serverProfileRepository.DeleteAsync(profileToRemove);

            server.Profiles.Remove(profileToRemove.Id);
            server.BannedUsers.Add(profileToRemove.UserId);
            await Context.Servers.UpdateAsync(server);
        }
    }
}