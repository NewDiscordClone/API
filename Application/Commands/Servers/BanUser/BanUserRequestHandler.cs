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
            
            if (server.Owner.Id != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You don't have the appropriate rights");
            
            ServerProfile userToRemove = server.ServerProfiles.Find(sp => sp.User.Id == request.UserId) ??
                                         throw new Exception("The user are not a member of the server");
            server.ServerProfiles.Remove(userToRemove);
            server.BannedUsers.Add(userToRemove.User.Id);
            
            await Context.Servers.UpdateAsync(server);
        }
    }
}