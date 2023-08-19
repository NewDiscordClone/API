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

            Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
                Owner = user
            };
            server.ServerProfiles.Add(new() { User = user, Server = server });

            await Context.Servers.AddAsync(server, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return server.Id;
        }

        public CreateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
