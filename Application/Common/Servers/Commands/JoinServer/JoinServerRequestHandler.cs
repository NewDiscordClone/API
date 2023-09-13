using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.JoinServer
{
    public class JoinServerRequestHandler : RequestHandlerBase, IRequestHandler<JoinServerRequest>
    {
        public JoinServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }

        public async Task Handle(JoinServerRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Invitation invitation = await Context.Invitations.FindAsync(request.InvitationId);
            if (invitation.ExpireTime < DateTime.Now)
            {
                await Context.Invitations.DeleteAsync(invitation);
                throw new NoPermissionsException("The invitation is expired");
            }
            Server server = await Context.Servers.FindAsync(invitation.ServerId);
            if (server.ServerProfiles.Any(sp => sp.UserId == UserId))
                throw new NoPermissionsException("You already a server member");
            if (server.BannedUsers.Contains(UserId))
                throw new NoPermissionsException("You are banned from the server");
            User user = await Context.SqlUsers.FindAsync(UserId);
            server.ServerProfiles.Add(new ServerProfile
            {
                UserId = UserId,
                Roles = new List<Role>(),
                DisplayName = user.DisplayName
            });
            await Context.Servers.UpdateAsync(server);
        }
    }
}