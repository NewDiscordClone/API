using MediatR;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Messages.Commands.RemoveMessage
{
    public class RemoveMessageCommandHandler : RequestHandlerBase, IRequestHandler<RemoveMessageCommand, Message>
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IServerProfileRepository _serverProfileRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;

        public async Task<Message> Handle(RemoveMessageCommand command, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.FindAsync(command.MessageId, cancellationToken);

            if (message.ChatId != command.ChatId)
                throw new InvalidOperationException("The specified message does not belong to the given chat.");

            Chat chat = await _chatRepository.FindAsync(message.ChatId, cancellationToken);

            UserProfile? profile = await _profileRepository
             .FindOrDefaultByChatIdAndUserIdAsync(chat.Id, UserId, true, cancellationToken);

            profile ??= await _serverProfileRepository
                .FindProfileByChannelIdAsync(chat.Id, UserId, cancellationToken);

            if (message.Author != UserId
                || UserProvider.HasClaims(profile!, Constants.Claims.RemoveMessages)
                || UserProvider.IsAdmin(profile!))
            {
                throw new NoPermissionsException("You don't have permission to remove the message");
            }

            //Remove image
            await _messageRepository.DeleteAsync(message, cancellationToken);
            return message;
        }

        public RemoveMessageCommandHandler(IAuthorizedUserProvider userProvider,
            IUserProfileRepository profileRepository,
            IServerProfileRepository serverProfileRepository,
            IMessageRepository messageRepository,
            IChatRepository chatRepository)
            : base(userProvider)
        {
            _profileRepository = profileRepository;
            _serverProfileRepository = serverProfileRepository;
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
        }
    }
}