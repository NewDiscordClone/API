using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.UnbanUser
{
    public class UnbanUserRequestHandler: RequestHandlerBase, IRequestHandler<UnbanUserRequest>
    {
        public UnbanUserRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task Handle(UnbanUserRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);
            
            if (server.Owner != UserId) //TODO: Замінити на логіку перевірки claim
                throw new NoPermissionsException("You don't have the appropriate rights");
            
            server.BannedUsers.Remove(request.UserId);
            
            await Context.Servers.UpdateAsync(server);
        }
    }
}