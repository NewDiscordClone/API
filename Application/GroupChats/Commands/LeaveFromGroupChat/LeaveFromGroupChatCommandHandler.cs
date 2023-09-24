using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.LeaveFromGroupChat
{
    public class LeaveFromGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<LeaveFromGroupChatCommand>
    {

        private readonly IUserProfileRepository _userProfileRepository;

        public async Task Handle(LeaveFromGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            UserProfile profile = await _userProfileRepository
                .FindAsync(command.ProfileId);

            await _userProfileRepository.DeleteAsync(profile);

            if (chat.Profiles.Count <= 1)
            {
                await Context.GroupChats.DeleteAsync(chat);
                await Context.Messages.DeleteManyAsync(message => message.ChatId == chat.Id);
                return;
            }

            chat.Profiles.Remove(profile.Id);
            if (chat.OwnerId == profile.Id)
                chat.OwnerId = chat.Profiles.First();

            await Context.GroupChats.UpdateAsync(chat);
        }

        public LeaveFromGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IUserProfileRepository userProfileRepository) : base(
            context, userProvider)
        {
            _userProfileRepository = userProfileRepository;
        }
    }
}