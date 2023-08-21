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
            Server server = await Context.FindByIdAsync<Server>
                (request.ServerId, cancellationToken);

            Context.Servers.Remove(server);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public DeleteServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
