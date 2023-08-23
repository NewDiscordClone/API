using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Servers.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerRequest>
    {

        public async Task Handle(DeleteServerRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);
            Server server = await Context.FindSqlByIdAsync<Server>
                (request.ServerId, cancellationToken);

            if (user.Id != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");
            Context.Servers.Remove(server);
            await Context.Channels.DeleteManyAsync(Builders<Channel>.Filter.Eq(c => c.ServerId, request.ServerId),null, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
