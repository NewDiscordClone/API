using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.RegularExpressions;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Commands
{
    public class ChangeAvatarCommandHandler : RequestHandlerBase, IRequestHandler<ChangeAvatarCommand, User>
    {
        private readonly IRepository<User, Guid> _repository;
        public ChangeAvatarCommandHandler(IAppDbContext context,
            IAuthorizedUserProvider userProvider,
            IRepository<User, Guid> repository) : base(context, userProvider)
        {
            _repository = repository;
        }

        public async Task<User> Handle(ChangeAvatarCommand command, CancellationToken cancellationToken)
        {
            User user = await _repository.FindAsync(UserId, cancellationToken);
            string? oldImage = user.Avatar;
            user.Avatar = command.AvatarUrl;

            await _repository.UpdateAsync(user, cancellationToken);

            if (oldImage != null)
            {
                string id = Regexes.ObjectIdRegex.Match(oldImage).Value;

                if (!Constants.User.DefaultAvatarIds.Contains(id))
                    await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/') - 1)..]);
            }

            return user;
        }
    }
}
