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
        public List<ChannelLookupDto> Channels { get; init; }
        public List<RoleDto> Roles { get; init; }

        public void Mapping(Profile profile)
        {
           profile.CreateMap<Server,ServerDetailsDto>();
        }
    }
}