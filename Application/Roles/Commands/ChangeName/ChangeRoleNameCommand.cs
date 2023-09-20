using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Commands.ChangeName
{
    public class ChangeRoleNameCommand : IRequest<Role>
    {
        public Guid RoleId { get; init; }
        public string Name { get; init; }
    }
}
