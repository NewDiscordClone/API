using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelCommandHandler(IAuthorizedUserProvider userProvider,
        IChatRepository chatRepository,
        IMessageRepository messageRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<RemoveChannelCommand, Channel>
    {
        private readonly IChatRepository _chatRepository = chatRepository;
        private readonly IMessageRepository _messageRepository = messageRepository;

        public async Task<Channel> Handle(RemoveChannelCommand command, CancellationToken cancellationToken)
        {
            Channel channel = await _chatRepository.FindAsync<Channel>(command.ChatId, cancellationToken);

            await _chatRepository.DeleteAsync(channel, cancellationToken);
            await _messageRepository.DeleteManyAsync(messages =>
                messages.Where(message => message.ChatId == channel.Id), cancellationToken);

            return channel;
        }
    }
}