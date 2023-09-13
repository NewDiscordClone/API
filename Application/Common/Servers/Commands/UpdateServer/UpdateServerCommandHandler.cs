using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.UpdateServer
{
    public class UpdateServerCommandHandler : RequestHandlerBase, IRequestHandler<UpdateServerRequest>
    {
        public async Task Handle(UpdateServerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(request.ServerId);

            if (UserId != server.Owner)
                throw new NoPermissionsException("You are not the owner of the server");

            if (request.Image != null && server.Image != null)
                await Context.CheckRemoveMedia(server.Image[(server.Image.LastIndexOf('/') - 1)..]);

            server.Title = request.Title ?? server.Title;
            server.Image = request.Image ?? server.Image;

            await Context.Servers.UpdateAsync(server);
        }

        public UpdateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}