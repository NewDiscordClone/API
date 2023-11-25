using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveReaction
{
    public class RemoveReactionCommandHandler : RequestHandler, IRequestHandler<RemoveReactionCommand, Message>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMessageRepository _messageRepository;

        public async Task<Message> Handle(RemoveReactionCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId, cancellationToken);
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
            await _messageRepository.UpdateAsync(message, cancellationToken);

            return message;
        }

        public RemoveReactionCommandHandler(IAuthorizedUserProvider userProvider,
            IUserProfileRepository userProfileRepository,
            IMessageRepository messageRepository) : base(userProvider)
        {
            _userProfileRepository = userProfileRepository;
            _messageRepository = messageRepository;
        }
    }
}