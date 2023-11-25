using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.CreateChannel
{
    public class CreateChannelCommandHandler(IAuthorizedUserProvider userProvider,
        IMapper mapper)
        : RequestHandler(userProvider, mapper), IRequestHandler<CreateChannelCommand, Channel>
    {

        private readonly IServerRepository _serverRepository;
        private readonly IChatRepository _chatRepository;

        public async Task<Channel> Handle(CreateChannelCommand command, CancellationToken cancellationToken)
        {
            Server server = await _serverRepository.FindAsync(command.ServerId, cancellationToken);

            Channel channel = new()
            {
                Title = command.Title,
                Profiles = server.Profiles,
                ServerId = server.Id,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _chatRepository.AddAsync(channel, cancellationToken);
            return channel;
        }
    }
}