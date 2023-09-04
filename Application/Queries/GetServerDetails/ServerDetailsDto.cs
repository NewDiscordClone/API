using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MongoDB.Bson;

namespace Application.Queries.GetServerDetails
{
    public record ServerDetailsDto : IMapWith<Server>
    {
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }
        [DefaultValue("ServerTitle")]
        public string Title { get; init; }
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
        public List<ServerProfileLookupDto> ServerProfiles { get; init; } = new();
        [NotMapped]
        public List<Channel> Channels { get; set; }
        public List<RoleDto> Roles { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, ServerDetailsDto>();
        }
    }
}