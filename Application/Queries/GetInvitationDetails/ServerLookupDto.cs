using Application.Interfaces;
using Application.Models;
using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Queries.GetInvitationDetails
{
    public record ServerLookupDto : IMapWith<Server>
    {
        /// <summary>
        /// The unique identifier of the server.
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }

        /// <summary>
        /// The title of the server.
        /// </summary>
        [DefaultValue("Title")]
        public string Title { get; init; }

        /// <summary>
        /// The URL of the image associated with the server. (Optional)
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, ServerLookupDto>();
        }
    }
}
