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
            Invitation invitation = new Invitation()
            {
                ServerId = request.ServerId,
                ExpireTime = request.ExpireTime,
                UserId = request.IncludeUser ? UserId: null
            };
            return (await Context.Invitations.AddAsync(invitation)).Id;
        }
    }
}