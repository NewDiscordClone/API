using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeDisplayNameCommandHandler : RequestHandlerBase, IRequestHandler<ChangeDisplayNameCommand, User>
    {
        private readonly IRepository<User, Guid> _userRepository;
        public ChangeDisplayNameCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IRepository<User, Guid> userRepository) : base(context, userProvider)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(ChangeDisplayNameCommand command, CancellationToken cancellationToken)
        {
            User user = await _userRepository.FindAsync(UserId, cancellationToken: cancellationToken);

            user.DisplayName = command.DisplayName;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}
