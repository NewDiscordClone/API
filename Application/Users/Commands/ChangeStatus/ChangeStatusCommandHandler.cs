using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeStatusCommandHandler(IAuthorizedUserProvider userProvider, IUserRepository userRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<ChangeStatusCommand, User>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User> Handle(ChangeStatusCommand command, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(UserId, cancellationToken);
            user.Status = command.Status;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}