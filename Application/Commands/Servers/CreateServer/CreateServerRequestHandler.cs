using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Servers.CreateServer
{
    public class CreateServerRequestHandler : RequestHandlerBase, IRequestHandler<CreateServerRequest, int>
    {
        private readonly IRoleFactory _roleFactory;

        public async Task<int> Handle(CreateServerRequest request, CancellationToken cancellationToken)
        {
            User user = await Context.FindByIdAsync<User>(UserId, cancellationToken);

            List<Role> roles = _roleFactory.GetDefaultServerRoles();

            Server server = new()
            {
                Title = request.Title,
                Image = request.Image,
                Roles = roles

            };
            server.ServerProfiles.Add(new()
            {
                User = user,
                Server = server,
                Roles = new() { roles.First(role => role.Name == "Owner") }
            });

            await Context.Servers.AddAsync(server, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return server.Id;
        }

        public CreateServerRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IRoleFactory roleFactory) : base(context, userProvider)
        {
            _roleFactory = roleFactory;
            // _roleFactory = roleFactory;
        }
    }
}
