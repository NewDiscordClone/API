using AutoMapper;
using Sparkle.Domain;
using Sparkle.Domain.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Servers.Queries.ServersList
{
    public record ServerLookUpDto : IMapWith<Server>
    {
        /// <summary>
        ///  The unique identifier of the server
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }

        /// <summary>
        /// Server's name
        /// </summary>
        [DefaultValue("Title")]
        public string Title { get; init; }

        /// <summary>
        /// Avatar Url of the Server
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, ServerLookUpDto>();
        }
    }
}