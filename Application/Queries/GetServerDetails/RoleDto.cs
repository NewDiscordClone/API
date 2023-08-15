using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public record RoleDto : IMapWith<Role>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDto>();
        }
    }
}