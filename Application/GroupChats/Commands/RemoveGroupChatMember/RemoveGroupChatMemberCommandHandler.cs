using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.RemoveGroupChatMember
{
    public class RemoveGroupChatMemberCommandHandler : RequestHandlerBase,
        IRequestHandler<RemoveGroupChatMemberCommand>
    {
        public async Task Handle(RemoveGroupChatMemberCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            UserProfile? profile = chat.Profiles.Find(p => p.UserId == command.MemberId)
                ?? throw new NoSuchUserException();

            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("You are not an owner of the chat");

            if (UserId == command.MemberId)
                throw new Exception("You can't remove yourself");

            chat.Profiles.Remove(profile);

            await Context.GroupChats.UpdateAsync(chat);
        }

        public RemoveGroupChatMemberCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) :
            base(context, userProvider)
        {
        }
    }
}