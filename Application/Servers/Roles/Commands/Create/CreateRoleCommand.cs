using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Servers.Roles.Commands.Create
{
    public record CreateRoleCommand : IRequest<Role>, IMapWith<Role>
    {
        public string Name { get; init; }
        public string Color { get; init; }
        public string ServerId { get; init; }
        public IEnumerable<IdentityRoleClaim<Guid>> Claims { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRoleCommand, Role>();
        }
    }
}
