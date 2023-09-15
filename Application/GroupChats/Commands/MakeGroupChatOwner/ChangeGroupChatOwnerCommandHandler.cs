using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.MakeGroupChatOwner
{
    public class ChangeGroupChatOwnerCommandHandler : RequestHandlerBase, IRequestHandler<ChangeGroupChatOwnerCommand>
    {
        public async Task Handle(ChangeGroupChatOwnerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat pchat = await Context.GroupChats.FindAsync(command.ChatId);
            if (pchat is not GroupChat chat)
                throw new Exception("This is not group chat");

            if (chat.OwnerId != UserId)
                throw new NoPermissionsException("User is not an owner of the chat");
            if (!chat.Users.Any(u => u == command.MemberId))
                throw new NoSuchUserException("User in not a member of the chat");

            chat.OwnerId = command.MemberId;
            await Context.GroupChats.UpdateAsync(chat);
        }

        public ChangeGroupChatOwnerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}