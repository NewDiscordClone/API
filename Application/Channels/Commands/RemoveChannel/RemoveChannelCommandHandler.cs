using MediatR;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelCommandHandler : RequestHandlerBase, IRequestHandler<RemoveChannelCommand>
    {
        public async Task Handle(RemoveChannelCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Channel chat = await Context.Channels.FindAsync(command.ChatId);

            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            await Context.Chats.DeleteAsync(chat);
        }

        public RemoveChannelCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}