using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeStatusCommandHandler : RequestHandlerBase, IRequestHandler<ChangeStatusCommand>
    {
        public ChangeStatusCommandHandler(
            IAppDbContext context,
            IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }

        public async Task Handle(ChangeStatusCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            User user = await Context.SqlUsers.FindAsync(UserId);
            user.Status = command.Status;
            await Context.SqlUsers.UpdateAsync(user);
        }
    }
}