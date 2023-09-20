using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.Create
{
    public class CreateRoleCommandHandler : RequestHandlerBase, IRequestHandler<CreateRoleCommand, Role>
    {
        public CreateRoleCommandHandler(IAppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId);

            Role role = Mapper.Map<Role>(command);
            await Context.AddClaimsToRoleAsync(role, command.Claims);

            server.Roles.Add(role.Id);

            await Context.Servers.UpdateAsync(server);
            await Context.SaveChangesAsync();
            return role;
        }
    }
}
