using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionCommandHandler : RequestHandlerBase, IRequestHandler<RemoveReactionCommand>
    {
        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userProfileRepository;
        public async Task Handle(RemoveReactionCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Message message = await Context.Messages.FindAsync(command.MessageId, cancellationToken);
            Reaction reaction;

            try
            {
                reaction = message.Reactions[command.ReactionIndex];
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException($"Reaction by {command.ReactionIndex} index does not exists");
            }

            UserProfile profile = await _userProfileRepository.FindAsync(reaction.AuthorProfile, cancellationToken);

            if (UserId != profile.UserId)
                throw new NoPermissionsException("This isn't your reaction");

            message.Reactions.RemoveAt(command.ReactionIndex);

            await Context.Messages.UpdateAsync(message, cancellationToken);
        }

        public RemoveReactionCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, Common.Interfaces.Repositories.IUserProfileRepository userProfileRepository) : base(context,
            userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}