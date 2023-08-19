using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.DeleteServer
{
    public class DeleteServerRequestHandler : RequestHandlerBase, IRequestHandler<DeleteServerRequest>
    {

        public async Task Handle(DeleteServerRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            Server server = await Context.FindByIdAsync<Server>
                (request.ServerId, cancellationToken);

            if (user.Id != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");
            Context.Servers.Remove(server);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
