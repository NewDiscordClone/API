using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Invitations.MakeInvitation
{
    public class MakeInvitationRequestHandler : RequestHandlerBase, IRequestHandler<MakeInvitationRequest, string>
    {
        public MakeInvitationRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }

        public async Task<string> Handle(MakeInvitationRequest request, CancellationToken cancellationToken)
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