using AutoMapper;
using Sparkle.Domain;
using Sparkle.Domain.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sparkle.Application.Servers.Queries.ServerDetails
{
    public record ServerDetailsDto : IMapWith<Server>
    {
        /// <summary>
        /// The unique identifier for the server.
        /// </summary>
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }

        /// <summary>
        /// The non-unique title of the server.
        /// </summary>
        [DefaultValue("ServerTitle")]
        public string Title { get; init; }

        /// <summary>
        /// The URL of the server's image.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }

        /// <summary>
        /// List of user profiles on this server.
        /// </summary>
        public List<Guid> ServerProfiles { get; init; } = new();

        /// <summary>
        /// List of channels on this server. (Not mapped to the database.)
        /// </summary>
        [NotMapped]
        public List<Channel> Channels { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, ServerDetailsDto>()
                .ForMember(p => p.ServerProfiles, opt => opt.MapFrom(s => s.Profiles));
        }
    }
}
