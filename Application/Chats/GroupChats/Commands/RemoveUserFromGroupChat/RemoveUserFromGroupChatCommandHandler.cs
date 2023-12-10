using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Chats.GroupChats.Commands.RemoveUserFromGroupChat
{
    public class RemoveUserFromGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<RemoveUserFromGroupChatCommand,
RemoveUserFromGroupChatCommandResult>
    {

        private readonly Common.Interfaces.Repositories.IUserProfileRepository _userProfileRepository;

        public async Task<RemoveUserFromGroupChatCommandResult> Handle(RemoveUserFromGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId, cancellationToken);
            if (pchat is not GroupChat chat)
                throw new InvalidOperationException("This is not group chat");


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
                await Context.GroupChats.DeleteAsync(chat, cancellationToken);
                await Context.Messages.DeleteManyAsync(message => message.ChatId == chat.Id, cancellationToken);
                return new(chat, profile.UserId);
            }

            chat.Profiles.Remove(profile.Id);
            if (chat.OwnerId == profile.Id)
            {
                chat.OwnerId = chat.Profiles.First();
                await _userProfileRepository.SetGroupChatOwner(chat.OwnerId, cancellationToken);
            }

            await Context.GroupChats.UpdateAsync(chat, cancellationToken);

            return new(chat, profile.UserId);
        }

        public RemoveUserFromGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, Common.Interfaces.Repositories.IUserProfileRepository userProfileRepository) : base(
            context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}