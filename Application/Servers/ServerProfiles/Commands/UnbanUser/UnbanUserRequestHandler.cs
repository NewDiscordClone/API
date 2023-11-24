using MediatR;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.ServerProfiles.Commands.UnbanUser
{
    public class UnbanUserRequestHandler(IServerRepository repository) : IRequestHandler<UnbanUserCommand>
    {
        private readonly IServerRepository _repository = repository;

        public async Task Handle(UnbanUserCommand request, CancellationToken cancellationToken)
        {
            Server server = await _repository.FindAsync(request.ServerId, cancellationToken);

            server.BannedUsers.Remove(request.UserId);

            await _repository.UpdateAsync(server, cancellationToken);
        }
    }
}