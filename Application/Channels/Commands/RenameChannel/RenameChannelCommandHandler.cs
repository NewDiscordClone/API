using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.RenameChannel
{
    public class RenameChannelCommandHandler(IAuthorizedUserProvider userProvider, IChatRepository chatRepository)
        : RequestHandler(userProvider), IRequestHandler<RenameChannelCommand, Channel>
    {
        private readonly IChatRepository _chatRepository = chatRepository;
        public async Task<Channel> Handle(RenameChannelCommand command, CancellationToken cancellationToken)
        {
            Channel channel = await _chatRepository.FindAsync<Channel>(command.ChatId, cancellationToken);
            channel.Title = command.NewTitle;

            await _chatRepository.UpdateAsync(channel, cancellationToken);
            return channel;
        }
    }
}