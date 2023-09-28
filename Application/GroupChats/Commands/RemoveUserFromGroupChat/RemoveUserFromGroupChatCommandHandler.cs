using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.RemoveUserFromGroupChat
{
    public class RemoveUserFromGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<RemoveUserFromGroupChatCommand>
    {

        private readonly IUserProfileRepository _userProfileRepository;

        public async Task Handle(RemoveUserFromGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId, cancellationToken);
            if (pchat is not GroupChat chat)
                throw new InvalidOperationException("This is not group chat");

            UserProfile profile = await _userProfileRepository
                .FindAsync(command.ProfileId, cancellationToken);

            await _userProfileRepository.DeleteAsync(profile, cancellationToken);

            if (chat.Profiles.Count <= 1)
            {
                await Context.GroupChats.DeleteAsync(chat, cancellationToken);
                await Context.Messages.DeleteManyAsync(message => message.ChatId == chat.Id, cancellationToken);
                return;
            }

            chat.Profiles.Remove(profile.Id);
            if (chat.OwnerId == profile.Id)
            {
                chat.OwnerId = chat.Profiles.First();
                await _userProfileRepository.SetGroupChatOwner(chat.OwnerId, cancellationToken);
            }

            await Context.GroupChats.UpdateAsync(chat, cancellationToken);
        }

        public RemoveUserFromGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IUserProfileRepository userProfileRepository) : base(
            context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}