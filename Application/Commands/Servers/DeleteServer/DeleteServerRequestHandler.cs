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
            Server server = await Context.FindByIdAsync<Server>
                (request.ServerId, cancellationToken);

            if (UserId != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");
            
            await Context.Servers.DeleteOneAsync(Context.GetIdFilter<Server>(request.ServerId), cancellationToken);
            await Context.Channels.DeleteManyAsync(Builders<Channel>.Filter.Eq(c => c.ServerId, request.ServerId),null, cancellationToken);
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
