using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeTextStatusCommandHandler(IAuthorizedUserProvider userProvider,
        IUserRepository repository)
        : RequestHandlerBase(userProvider), IRequestHandler<ChangeTextStatusCommand, User>
    {
        private readonly IUserRepository _repository = repository;

        public async Task<User> Handle(ChangeTextStatusCommand command, CancellationToken cancellationToken)
        {
            User user = await _repository.FindAsync(UserId, cancellationToken);

            user.TextStatus = command.TextStatus;

            await _repository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}
