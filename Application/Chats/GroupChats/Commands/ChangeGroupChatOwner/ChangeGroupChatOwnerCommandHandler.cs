using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.ChangeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatOwnerCommand>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public async Task Handle(ChangeGroupChatOwnerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId, cancellationToken);

            if (pchat is not GroupChat chat)
                throw new InvalidOperationException("This is not group chat");

            if (chat.OwnerId == command.ProfileId)
                return;

            await _userProfileRepository.DeleteGroupChatOwner(chat.OwnerId, cancellationToken);
            await _userProfileRepository.SetGroupChatOwner(command.ProfileId, cancellationToken);

            chat.OwnerId = command.ProfileId;

            await Context.GroupChats.UpdateAsync(chat, cancellationToken);
        }

        public ChangeGroupChatOwnerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IUserProfileRepository userProfileRepository) : base(
            context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}