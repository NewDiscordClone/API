using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MongoDB.Bson;

namespace Application.Commands.Invitations.GetInvitationDetails
{
    public record ServerLookupDto : IMapWith<Server>
    {
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }
        [DefaultValue("Title")]
        public string Title { get; init; }
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, ServerLookupDto>();
        }
    }
}