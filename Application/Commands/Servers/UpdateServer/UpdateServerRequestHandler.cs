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
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            Server server = await Context.FindByIdAsync<Server>
                (request.ServerId, cancellationToken);

            server.Title = request.Title ?? server.Title;
            server.Image = request.Image ?? server.Image;

            Context.Servers.Update(server);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public UpdateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
