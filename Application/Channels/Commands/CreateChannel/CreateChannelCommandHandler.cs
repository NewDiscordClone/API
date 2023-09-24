using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.CreateChannel
{
    public class CreateChannelCommandHandler : RequestHandlerBase, IRequestHandler<CreateChannelCommand, string>
    {
        public async Task<string> Handle(CreateChannelCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId);

            Channel channel = new()
            {
                Title = command.Title,
                Profiles = server.Profiles,
                ServerId = server.Id
            };

            await Context.Channels.AddAsync(channel);
            return channel.Id;
        }

        public CreateChannelCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}