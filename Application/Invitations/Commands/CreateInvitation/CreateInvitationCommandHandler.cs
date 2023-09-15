using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Invitations.Commands.CreateInvitation
{
    public class CreateInvitationCommandHandler : RequestHandlerBase, IRequestHandler<CreateInvitationCommand, string>
    {
        public CreateInvitationCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task<string> Handle(CreateInvitationCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(command.ServerId);

            //TODO: Перевірити на те що у юзера є відповідний клейм
            if (!server.ServerProfiles.Any(u => u.UserId == UserId))
                throw new NoPermissionsException("You are not a member of the server");

            Invitation invitation = new()
            {
                ServerId = command.ServerId,
                ExpireTime = command.ExpireTime,
                UserId = command.IncludeUser ? UserId : null
            };
            return (await Context.Invitations.AddAsync(invitation)).Id;
        }
    }
}