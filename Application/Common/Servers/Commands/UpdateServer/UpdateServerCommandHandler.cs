using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.UpdateServer
{
    public class UpdateServerCommandHandler : RequestHandlerBase, IRequestHandler<UpdateServerCommand>
    {
        public async Task Handle(UpdateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId);

            if (UserId != server.Owner)
                throw new NoPermissionsException("You are not the owner of the server");

            if (command.Image != null && server.Image != null)
                await Context.CheckRemoveMedia(server.Image[(server.Image.LastIndexOf('/') - 1)..]);

            server.Title = command.Title ?? server.Title;
            server.Image = command.Image ?? server.Image;

            await Context.Servers.UpdateAsync(server);
        }

        public UpdateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}