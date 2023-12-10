using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Chats.GroupChats.Commands.ChangeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatOwnerCommand, Chat>
    {
        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userProfileRepository;
        public async Task<Chat> Handle(ChangeGroupChatOwnerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId, cancellationToken);

            if (pchat is not GroupChat chat)
                throw new InvalidOperationException("This is not group chat");

            if (chat.OwnerId == command.ProfileId)
                return chat;

            await _userProfileRepository.DeleteGroupChatOwner(chat.OwnerId, cancellationToken);
            await _userProfileRepository.SetGroupChatOwner(command.ProfileId, cancellationToken);

            chat.OwnerId = command.ProfileId;

            await Context.GroupChats.UpdateAsync(chat, cancellationToken);

            return chat;
        }

        public ChangeGroupChatOwnerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, Common.Interfaces.Repositories.IUserProfileRepository userProfileRepository) : base(
            context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}