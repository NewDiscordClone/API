using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.RenameChannel
{
    public class RenameChannelCommandHandler : RequestHandlerBase, IRequestHandler<RenameChannelCommand>
    {
        public async Task Handle(RenameChannelCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Channel chat = await Context.Channels.FindAsync(command.ChatId);

            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = command.NewTitle;

            await Context.Channels.UpdateAsync(chat);
        }

        public RenameChannelCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}