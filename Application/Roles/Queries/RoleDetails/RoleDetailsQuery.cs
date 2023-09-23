using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Roles.Queries.RoleDetails
{
    public record RoleDetailsQuery : IRequest<Role>
    {
        public Guid RoleId { get; init; }
    }
}
