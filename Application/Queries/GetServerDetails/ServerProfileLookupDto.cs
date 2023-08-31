using System.ComponentModel;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public record ServerProfileLookupDto : IMapWith<ServerProfile>
    {
        [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string Name { get; init; }
        [DefaultValue(1)]
        public int UserId { get; init; }
        
        public RoleDto? MainRole { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ServerProfile,
                ServerProfileLookupDto>();
        }
    }
}