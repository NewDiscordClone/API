using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Invitations.CreateInvitation
{
    public class CreateInvitationRequestHandler : RequestHandlerBase, IRequestHandler<CreateInvitationRequest, string>
    {
        public CreateInvitationRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task<string> Handle(CreateInvitationRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Server server = await Context.Servers.FindAsync(request.ServerId);

            //TODO: Перевірити на те що у юзера є відповідний клейм
            if (!server.ServerProfiles.Any(u => u.UserId == UserId))
                throw new NoPermissionsException("You are not a member of the server");

            Invitation invitation = new()
            {
                ServerId = request.ServerId,
                ExpireTime = request.ExpireTime,
                UserId = request.IncludeUser ? UserId : null
            };
            return (await Context.Invitations.AddAsync(invitation)).Id;
        }
    }
}