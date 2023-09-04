using System.ComponentModel;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public record RoleDto : IMapWith<Role>
    {
        public Guid Id { get; init; }
        [DefaultValue("Admin")]
        public string Name { get; init; }
        [DefaultValue("#FF0000")]
        public string Color { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDto>();
        }
    }
}