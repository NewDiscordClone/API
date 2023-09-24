using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public class CreateServerCommandHandler : RequestHandlerBase, IRequestHandler<CreateServerCommand, string>
    {
        public async Task<string> Handle(CreateServerCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            //TODO: Добавить роли
            Server server = new()
            {
                Title = command.Title,
                Image = command.Image,
                Owner = UserId,
            };

            //TODO: Добавить роли
            ServerProfile owner = new()
            {
                UserId = UserId,
                ServerId = server.Id
            };

            server.Owner = owner.Id;
            server.Profiles.Add(owner.Id);

            await Context.UserProfiles.AddAsync(owner, cancellationToken);
            await Context.Servers.AddAsync(server);
            return server.Id;
        }

        public CreateServerCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
            : base(context, userProvider, mapper)
        {
        }
    }
}