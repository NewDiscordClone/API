using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using RemoveChatUserResult = (Sparkle.Application.Models.GroupChat Chat, System.Guid RemovedUserId);

namespace Sparkle.Application.Chats.GroupChats.Commands.RemoveUserFromGroupChat
{
    public class RemoveUserFromGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<RemoveUserFromGroupChatCommand, RemoveChatUserResult>
    {

        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;

        public async Task<RemoveChatUserResult> Handle(RemoveUserFromGroupChatCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = await _chatRepository.FindAsync<GroupChat>(command.ChatId, cancellationToken);

            UserProfile profile;
            if (command.ProfileId is not null)
            {
                profile = await _userProfileRepository
                    .FindAsync(command.ProfileId.Value, cancellationToken);
            }
            else
            {
                profile = await _userProfileRepository
                    .FindByChatIdAndUserIdAsync(chat.Id, UserId, cancellationToken);
            }

            await _userProfileRepository.DeleteAsync(profile, cancellationToken);

            if (chat.Profiles.Count <= 1)
            {
                await _chatRepository.DeleteAsync(chat, cancellationToken);
                await _messageRepository.DeleteManyAsync(message
                    => message.ChatId == chat.Id, cancellationToken);

                return new(chat, profile.UserId);
            }

            chat.Profiles.Remove(profile.Id);
            if (chat.OwnerId == profile.Id)
            {
                chat.OwnerId = chat.Profiles.First();
                await _userProfileRepository.SetGroupChatOwner(chat.OwnerId, cancellationToken);
            }

            await _chatRepository.UpdateAsync(chat, cancellationToken);

            return new(chat, profile.UserId);
        }

        public RemoveUserFromGroupChatCommandHandler(IAuthorizedUserProvider userProvider,
            IUserProfileRepository userProfileRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository) : base(userProvider)
        {
            _userProfileRepository = userProfileRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
        }
    }
}