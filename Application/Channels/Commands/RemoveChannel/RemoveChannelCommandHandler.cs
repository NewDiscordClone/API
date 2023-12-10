using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelCommandHandler : RequestHandlerBase, IRequestHandler<RemoveChannelCommand, Channel>
    {
        public async Task<Channel> Handle(RemoveChannelCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Channel channel = await Context.Channels.FindAsync(command.ChatId, cancellationToken);

            await Context.Chats.DeleteAsync(channel, cancellationToken);
            await Context.Messages.DeleteManyAsync(message => message.ChatId == channel.Id, cancellationToken);

            return channel;
        }

        public RemoveChannelCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(
            context, userProvider)
        {
        }
    }
}