using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Invitations.Queries.InvitationDetails
{
    public class InvitationDetailsQueryHandler : RequestHandlerBase, IRequestHandler<InvitationDetailsQuery, InvitationDetailsDto>
    {
        public InvitationDetailsQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) : base(context, userProvider, mapper)
        {
        }

        public async Task<InvitationDetailsDto> Handle(InvitationDetailsQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            Invitation invitation = await Context.Invitations.FindAsync(query.InvitationId);
            if (invitation.ExpireTime is not null &&
                   invitation.ExpireTime < DateTime.Now)
            {
                await Context.Invitations.DeleteAsync(invitation);
                throw new NoPermissionsException("The invitation is expired");
            }
            User? user = invitation.UserId == null ? null : await Context.SqlUsers.FindAsync(invitation.UserId.Value);
            Server server = await Context.Servers.FindAsync(invitation.ServerId);

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