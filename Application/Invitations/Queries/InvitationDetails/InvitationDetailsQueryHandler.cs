using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Invitations.Queries.InvitationDetails
{
    public class InvitationDetailsQueryHandler : RequestHandler, IRequestHandler<InvitationDetailsQuery, InvitationDetailsDto>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServerRepository _serverRepository;

        public InvitationDetailsQueryHandler(IAuthorizedUserProvider userProvider,
            IMapper mapper,
            IInvitationRepository invitationRepository,
            IUserRepository userRepository,
            IServerRepository serverRepository)
            : base(userProvider, mapper)
        {
            _invitationRepository = invitationRepository;
            _userRepository = userRepository;
            _serverRepository = serverRepository;
        }

        public async Task<InvitationDetailsDto> Handle(InvitationDetailsQuery query, CancellationToken cancellationToken)
        {
            Invitation invitation = await _invitationRepository.FindAsync(query.InvitationId, cancellationToken);

            if (invitation.ExpireTime is not null &&
                   invitation.ExpireTime < DateTime.Now)
            {
                await _invitationRepository.DeleteAsync(invitation);
                throw new NoPermissionsException("The invitation is expired");
            }

            User? user = invitation.UserId == null
                ? null
                : await _userRepository.FindAsync(invitation.UserId.Value, cancellationToken);

            Server server = await _serverRepository.FindAsync(invitation.ServerId);

            return new InvitationDetailsDto
            {
                Id = query.InvitationId,
                Server = Mapper.Map<ServerLookupDto>(server),
                User = user == null ? null : Mapper.Map<UserViewModel>(user),
                ExpireTime = invitation.ExpireTime
            };
        }
    }
}