using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Queries.ServerDetails;

namespace Sparkle.Application.Servers.Commands.JoinServer
{
    public class JoinServerCommandHandler : RequestHandlerBase, IRequestHandler<JoinServerCommand, ServerDetailsDto>
    {
        private readonly IServerProfileRepository _serverProfileRepository;
        public JoinServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper, IServerProfileRepository serverProfileRepository) : base(context, userProvider, mapper)
        {
            _serverProfileRepository = serverProfileRepository;
        }

        public async Task<ServerDetailsDto> Handle(JoinServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Invitation invitation = await Context.Invitations.FindAsync(command.InvitationId);
            if (invitation.ExpireTime < DateTime.Now)
            {
                await Context.Invitations.DeleteAsync(invitation);
                throw new NoPermissionsException("The invitation is expired");
            }

            Server server = await Context.Servers.FindAsync(invitation.ServerId);


            if (_serverProfileRepository.IsUserServerMember(server.Id, UserId))
                throw new NoPermissionsException("You already a server member");

            if (server.BannedUsers.Contains(UserId))
                throw new NoPermissionsException("You are banned from the server");

            User user = await Context.SqlUsers.FindAsync(UserId);

            //TODO Добавить роли новому пользователю
            ServerProfile profile = new()
            {
                UserId = UserId,
                DisplayName = user.DisplayName,
                ServerId = server.Id,
            };

            server.Profiles.Add(profile.Id);
            await _serverProfileRepository.AddAsync(profile);
            await Context.Servers.UpdateAsync(server);

            return Mapper.Map<ServerDetailsDto>(await Context.Servers.UpdateAsync(server));
        }
    }
}