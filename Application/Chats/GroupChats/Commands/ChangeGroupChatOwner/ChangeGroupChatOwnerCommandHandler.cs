using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatOwnerCommand, GroupChat>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IChatRepository _chatRepository;
        public async Task<GroupChat> Handle(ChangeGroupChatOwnerCommand command, CancellationToken cancellationToken)
        {
            GroupChat chat = await _chatRepository.FindAsync<GroupChat>(command.ChatId, cancellationToken);

            if (chat.OwnerId == command.ProfileId)
                return chat;

            await _userProfileRepository.DeleteGroupChatOwner(chat.OwnerId, cancellationToken);
            await _userProfileRepository.SetGroupChatOwner(command.ProfileId, cancellationToken);

            chat.OwnerId = command.ProfileId;

            await _chatRepository.UpdateAsync(chat, cancellationToken);

            return chat;
        }

        public ChangeGroupChatOwnerCommandHandler(IAuthorizedUserProvider userProvider,
            IUserProfileRepository userProfileRepository,
            IChatRepository chatRepository)
            : base(userProvider)
        {
            _userProfileRepository = userProfileRepository;
            _chatRepository = chatRepository;
        }
    }
}