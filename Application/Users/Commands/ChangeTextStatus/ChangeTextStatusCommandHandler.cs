using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeTextStatusCommandHandler : RequestHandlerBase, IRequestHandler<ChangeTextStatusCommand, User>
    {
        private readonly IRepository<User, Guid> _repository;
        public ChangeTextStatusCommandHandler(IAuthorizedUserProvider userProvider, IRepository<User, Guid> repository) : base(userProvider)
        {
            _repository = repository;
        }

        public async Task<User> Handle(ChangeTextStatusCommand command, CancellationToken cancellationToken)
        {
            User user = await _repository.FindAsync(UserId, cancellationToken);

            user.TextStatus = command.TextStatus;

            await _repository.UpdateAsync(user, cancellationToken);

            return user;
        }
    }
}
