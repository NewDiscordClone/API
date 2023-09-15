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

            //TODO: Перевірити що у юзера є відповідні права
            Server server = await Context.Servers.FindAsync(command.ServerId);
            List<Guid> users = new();
            server.ServerProfiles.ForEach(profile => users.Add(profile.UserId));
            Channel channel = new()
            {
                Title = command.Title,
                Users = users,
                ServerId = server.Id
            };

            return (await Context.Channels.AddAsync(channel)).Id;
        }

        public CreateChannelCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}