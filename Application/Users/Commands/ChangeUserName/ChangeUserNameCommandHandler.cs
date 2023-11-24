using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands.ChangeUserName
{
    public class ChangeUserNameCommandHandler(IAuthorizedUserProvider userProvider,
        IUserRepository repository) : RequestHandlerBase(userProvider), IRequestHandler<ChangeUserNameCommand, User>
    {
        private readonly IUserRepository _repository = repository;

        public async Task<User> Handle(ChangeUserNameCommand command, CancellationToken cancellationToken)
        {
            User user = await _repository.FindAsync(UserId, cancellationToken);

            user.UserName = command.Username;

            await _repository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}
