using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Create
{
    public class CreateRoleCommandHandler : RequestHandlerBase, IRequestHandler<CreateRoleCommand, Role>
    {
        private readonly IRoleRepository _roleRepository;
        public CreateRoleCommandHandler(IAppDbContext context, IMapper mapper, IRoleRepository roleRepository) : base(context, mapper)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Server server = await Context.Servers.FindAsync(command.ServerId, cancellationToken);

            Role role = Mapper.Map<Role>(command);
            foreach (IdentityRoleClaim<Guid> claims in command.Claims)
            {
                claims.RoleId = role.Id;
            }

            await _roleRepository.AddClaimsToRoleAsync(role, command.Claims, cancellationToken);

            server.Roles.Add(role.Id);

            await _roleRepository.AddAsync(role, cancellationToken);
            await Context.Servers.UpdateAsync(server, cancellationToken);
            return role;
        }
    }
}
