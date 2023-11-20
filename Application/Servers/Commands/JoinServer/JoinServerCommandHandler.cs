using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.JoinServer
{
    public class JoinServerCommandHandler : RequestHandlerBase, IRequestHandler<JoinServerCommand, ServerProfile>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IRoleFactory _roleFactory;
        public JoinServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, IServerProfileRepository serverProfileRepository, IRoleFactory roleFactory) : base(context, userProvider, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
            _roleFactory = roleFactory;
        }

        public async Task<ServerProfile> Handle(JoinServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Invitation invitation = await Context.Invitations.FindAsync(command.InvitationId, cancellationToken);
            if (invitation.ExpireTime < DateTime.Now)
            {
                await Context.Invitations.DeleteAsync(invitation, cancellationToken);
                throw new NoPermissionsException("The invitation is expired");
            }

            Server server = await Context.Servers.FindAsync(invitation.ServerId, cancellationToken);


            if (_serverProfileRepository.IsUserServerMember(server.Id, UserId))
                throw new NoPermissionsException("You already a server member");

            if (server.BannedUsers.Contains(UserId))
                throw new NoPermissionsException("You are banned from the server");

            List<Channel> channels = await Context.Channels
                .FilterAsync(channel => channel.ServerId == server.Id, cancellationToken);

            Role memberRole = _roleFactory.ServerMemberRole;

            ServerProfile profile = new()
            {
                UserId = UserId,
                ServerId = server.Id,
                Roles = { memberRole }
            };

            server.Profiles.Add(profile.Id);
            channels.ForEach(channel => channel.Profiles.Add(profile.Id));
            await _serverProfileRepository.AddAsync(profile, cancellationToken);
            await Context.Servers.UpdateAsync(server, cancellationToken);

            return profile;
        }
    }
}