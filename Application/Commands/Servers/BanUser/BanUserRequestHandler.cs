using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.BanUser
{
    public class BanUserRequestHandler: RequestHandlerBase, IRequestHandler<BanUserRequest>
    {
        public BanUserRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(BanUserRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);
            
            if (server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You don't have the appropriate rights");

            ServerProfile? userToRemove = server.ServerProfiles.Find(sp => sp.UserId == request.UserId);
            if(userToRemove!=null)server.ServerProfiles.Remove(userToRemove);
            server.BannedUsers.Add(request.UserId);
            
            await Context.Servers.UpdateAsync(server);
        }
    }
}