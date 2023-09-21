using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Users.Commands.ChangeStatus
{
    public class ChangeStatusRequestHandler : RequestHandlerBase, IRequestHandler<ChangeStatusRequest>
    {
        public ChangeStatusRequestHandler(
            IAppDbContext context,
            IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }

        public async Task Handle(ChangeStatusRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            User user = await Context.SqlUsers.FindAsync(UserId);
            user.Status = request.Status;
            await Context.SqlUsers.UpdateAsync(user);
        }
    }
}