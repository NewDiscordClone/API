using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.GroupChats.Commands.RenameGroupChat
{
    public class RenameGroupChatCommandHandler : RequestHandlerBase, IRequestHandler<RenameGroupChatCommand>
    {
        public async Task Handle(RenameGroupChatCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            GroupChat chat = await Context.GroupChats.FindAsync(command.ChatId);
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = command.NewTitle;

            await Context.GroupChats.UpdateAsync(chat);
        }

        public RenameGroupChatCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}
