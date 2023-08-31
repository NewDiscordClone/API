using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerRequest, Server>
    {

        public async Task<Server> Handle(DeleteServerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(request.ServerId);

            if (UserId != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");

            await Context.Servers.DeleteAsync(server);
            await Context.Channels.DeleteManyAsync(c => c.ServerId == request.ServerId);

            return server;
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
