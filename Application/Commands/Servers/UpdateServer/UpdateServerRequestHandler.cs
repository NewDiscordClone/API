using Application.Exceptions;
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
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);
            Server server = await Context.FindSqlByIdAsync<Server>
                (request.ServerId, cancellationToken);
            
            if (user.Id != server.Owner.Id)
                throw new NoPermissionsException("You are not the owner of the server");
            
            server.Title = request.Title ?? server.Title;
            if(request.Image != null)
            {
                if(server.Image != null)
                    await Context.CheckRemoveMedia(server.Image[server.Image.LastIndexOf('/')..], cancellationToken);
                server.Image = request.Image;
            }

            Context.Servers.Update(server);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public UpdateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
