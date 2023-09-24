using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelCommandHandler : RequestHandlerBase, IRequestHandler<RemoveChannelCommand>
    {
        public async Task Handle(RemoveChannelCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Channel channel = await Context.Channels.FindAsync(command.ChatId);

            await Context.Chats.DeleteAsync(channel);
            await Context.Messages.DeleteManyAsync(message => message.ChatId == channel.Id);
        }

        public RemoveChannelCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}