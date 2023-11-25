using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeAvatarCommandHandler(IAuthorizedUserProvider userProvider,
        IUserRepository repository) : RequestHandler(userProvider), IRequestHandler<ChangeAvatarCommand, User>
    {
        private readonly IUserRepository _repository = repository;

        public async Task<User> Handle(ChangeAvatarCommand command, CancellationToken cancellationToken)
        {
            User user = await _repository.FindAsync(UserId, cancellationToken);
            string? oldImage = user.Avatar;
            user.Avatar = command.AvatarUrl;

            await _repository.UpdateAsync(user, cancellationToken);

            //Remove image
            //if (oldImage != null)
            //    await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);

            return user;
        }
    }
}
