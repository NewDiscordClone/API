using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeDisplayNameCommandHandler(IAuthorizedUserProvider userProvider,
        IUserRepository userRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<ChangeDisplayNameCommand, User>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User> Handle(ChangeDisplayNameCommand command, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(UserId, cancellationToken: cancellationToken);

            user.DisplayName = command.DisplayName;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}
