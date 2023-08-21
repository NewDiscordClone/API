using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.CreateServer
{
    public class CreateServerRequestHandler : RequestHandlerBase, IRequestHandler<CreateServerRequest, int>
    {
        public async Task<int> Handle(CreateServerRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            Role ownerRole = new()
            {
                Name = "Owner",
                Color = "#FFF000"
            };

            Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
                Roles = new() { ownerRole }

            };
            server.ServerProfiles.Add(new() { User = user, Server = server, Roles = new() { ownerRole } });

            await Context.Servers.AddAsync(server, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return server.Id;
        }

        public CreateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
