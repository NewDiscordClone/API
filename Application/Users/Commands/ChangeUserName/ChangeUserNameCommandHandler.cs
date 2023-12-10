using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands.ChangeUserName
{
    public class ChangeUserNameCommandHandler : RequestHandlerBase, IRequestHandler<ChangeUserNameCommand, User>
    {
        private readonly IRepository<User, Guid> _repository;
        public ChangeUserNameCommandHandler(IAuthorizedUserProvider userProvider,
            IRepository<User, Guid> repository)
            : base(userProvider)
        {
            _repository = repository;
        }

        public async Task<User> Handle(ChangeUserNameCommand command, CancellationToken cancellationToken)
        {
            User user = await _repository.FindAsync(UserId, cancellationToken);

            user.UserName = command.Username;

            await _repository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}
