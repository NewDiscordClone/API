using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.Channels.CreateChannel
{
    public class CreateChannelRequestHandler : RequestHandlerBase, IRequestHandler<CreateChannelRequest, Channel>
    {
        public async Task<Channel> Handle(CreateChannelRequest request, CancellationToken cancellationToken)
        {
            //TODO: Перевірити що у юзера є відповідні права
            Server server = await Context.FindByIdAsync<Server>(request.ServerId, cancellationToken);
            List<UserLookUp> users = new();
            server.ServerProfiles.ForEach(profile => users.Add(Mapper.Map<UserLookUp>(profile.User)));
            Channel channel = new()
            {
                Title = request.Title,
                Users = users,
                ServerId = server.Id
            };
            await Context.Chats.InsertOneAsync(channel, null, cancellationToken);
            return channel;
        }

        public CreateChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}