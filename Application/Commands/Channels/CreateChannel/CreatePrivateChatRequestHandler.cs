using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Channels.CreateChannel
{
    public class CreateChannelRequestHandler : RequestHandlerBase, IRequestHandler<CreateChannelRequest, Channel>
    {

        public async Task<Channel> Handle(CreateChannelRequest request, CancellationToken cancellationToken)
        {
            //TODO: Перевірити що у юзера є відповідні права
            Server server = await Context.FindByIdAsync<Server>(request.ServerId, cancellationToken, "ServerProfiles", "ServerProfiles.User");
            List<User> users = new();
            server.ServerProfiles.ForEach(profile => users.Add(profile.User));
            Channel channel = new()
            {
                Title = request.Title,
                Users = users,
                Server = server,
                Messages = new List<Message>()
            };

            await Context.Channels.AddAsync(channel, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return channel;
        }

        public CreateChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}