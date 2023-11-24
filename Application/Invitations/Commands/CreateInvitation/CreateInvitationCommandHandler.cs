using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Invitations.Commands.CreateInvitation
{
    public class CreateInvitationCommandHandler(IAuthorizedUserProvider userProvider,
        IInvitationRepository invitationRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<CreateInvitationCommand, Invitation>
    {
        private readonly IInvitationRepository _invitationRepository = invitationRepository;

        public async Task<Invitation> Handle(CreateInvitationCommand command, CancellationToken cancellationToken)
        {
            Invitation invitation = new()
            {
                ServerId = command.ServerId,
                ExpireTime = command.ExpireTime,
                UserId = command.IncludeUser ? UserId : null
            };

            await _invitationRepository.AddAsync(invitation, cancellationToken);

            return invitation;
        }
    }
}