using System.ComponentModel.DataAnnotations.Schema;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public record ServerDetailsDto : IMapWith<Server>
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string? Image { get; init; }
        public List<ServerProfileLookupDto> ServerProfiles { get; init; }
        [NotMapped]
        public List<Channel> Channels { get; set; }
        public List<RoleDto> Roles { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, ServerDetailsDto>();
        }
    }
}