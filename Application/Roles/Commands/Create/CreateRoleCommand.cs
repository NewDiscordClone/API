using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using System.Security.Claims;

namespace Sparkle.Application.Roles.Commands.Create
{
    public record CreateRoleCommand : IRequest<Role>, IMapWith<Role>
    {
        public string Name { get; init; }
        public string Color { get; init; }
        public string ServerId { get; init; }
        public int Priority { get; init; }
        public IEnumerable<Claim> Claims { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateRoleCommand, Role>();
        }
    }
}
