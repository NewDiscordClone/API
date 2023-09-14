using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Servers.Commands.CreateServer
{
    public class CreateServerCommandHandler : RequestHandlerBase, IRequestHandler<CreateServerCommand, string>
    {
        public async Task<string> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = new()
            {
                Title = command.Title,
                Image = command.Image,
                Owner = UserId,
                ServerProfiles = new List<ServerProfile>()
                {
                    new ServerProfile()
                    {
                        UserId = UserId
                    }
                }
            };

            server = await Context.Servers.AddAsync(server);
            return server.Id;
        }

        public CreateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}