using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Channels.Commands.CreateChannel
{
    public class CreateChannelRequestHandler : RequestHandlerBase, IRequestHandler<CreateChannelRequest, string>
    {
        public async Task<string> Handle(CreateChannelRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            //TODO: Перевірити що у юзера є відповідні права
            Server server = await Context.Servers.FindAsync(request.ServerId);
            List<Guid> users = new();
            server.ServerProfiles.ForEach(profile => users.Add(profile.UserId));
            Channel channel = new()
            {
                Title = request.Title,
                Users = users,
                ServerId = server.Id
            };

            return (await Context.Channels.AddAsync(channel)).Id;
        }

        public CreateChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}