using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.JoinServer
{
    public class JoinServerCommandHandler : RequestHandler, IRequestHandler<JoinServerCommand, ServerProfile>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IRoleFactory _roleFactory;
        private readonly IServerRepository _serverRepository;
        private readonly IChatRepository _chatRepository;

        public async Task<ServerProfile> Handle(JoinServerCommand command, CancellationToken cancellationToken)
        {
            Invitation invitation = await _invitationRepository
                .FindAsync(command.InvitationId, cancellationToken);

            if (invitation.ExpireTime < DateTime.Now)
            {
                await _invitationRepository.DeleteAsync(invitation, cancellationToken);
                throw new NoPermissionsException("The invitation is expired");
            }

            Server server = await _serverRepository.FindAsync(invitation.ServerId, cancellationToken);

            if (_serverProfileRepository.IsUserServerMember(server.Id, UserId))
                throw new NoPermissionsException("You already a server member");

            if (server.BannedUsers.Contains(UserId))
                throw new NoPermissionsException("You are banned from the server");

            List<Channel> channels = await _chatRepository.Channels
                .Where(channel => channel.ServerId == server.Id)
                .ToListAsync(cancellationToken);

            Role memberRole = _roleFactory.ServerMemberRole;

            ServerProfile profile = new()
            {
                UserId = UserId,
                ServerId = server.Id,
                Roles = { memberRole }
            };

            server.Profiles.Add(profile.Id);

            await _serverProfileRepository.AddAsync(profile, cancellationToken);
            await _serverRepository.UpdateAsync(server, cancellationToken);

            foreach (Channel channel in channels)
            {
                channel.Profiles.Add(profile.Id);
                await _chatRepository.UpdateAsync(channel, cancellationToken);
            }

            return profile;
        }

        public JoinServerCommandHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IServerProfileRepository serverProfileRepository,
            IRoleFactory roleFactory,
            IInvitationRepository invitationRepository,
            IServerRepository serverRepository) : base(userProvider, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
            _roleFactory = roleFactory;
            _invitationRepository = invitationRepository;
            _serverRepository = serverRepository;
        }
    }
}