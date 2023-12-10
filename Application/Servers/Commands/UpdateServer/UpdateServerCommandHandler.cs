using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Servers.Commands.UpdateServer
{
    public class UpdateServerCommandHandler : RequestHandlerBase, IRequestHandler<UpdateServerCommand>
    {
        public async Task Handle(UpdateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId, cancellationToken);

            if (command.Image != null && server.Image != null)
                await Context.CheckRemoveMedia(server.Image[(server.Image.LastIndexOf('/') - 1)..]);

            server.Title = command.Title ?? server.Title;
            server.Image = command.Image ?? server.Image;

            await Context.Servers.UpdateAsync(server, cancellationToken);
        }

        public UpdateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}