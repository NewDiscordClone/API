using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.UpdateServer
{
    public class UpdateServerRequestHandler : RequestHandlerBase, IRequestHandler<UpdateServerRequest>
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

        public UpdateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }
    }
}