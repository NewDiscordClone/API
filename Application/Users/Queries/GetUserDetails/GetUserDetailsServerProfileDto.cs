using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using System.ComponentModel;

namespace Application.Users.Queries.GetUserDetails
{
    public record GetUserDetailsServerProfileDto : IMapWith<ServerProfile>
    {
        /// <summary>
        /// User's username displayed on this server.
        /// </summary>
        [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string? DisplayName { get; set; }

        /// <summary>
        /// List of user's roles on this server.
        /// </summary>
        public virtual List<GetUserDetailsRolesDto> Roles { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ServerProfile, GetUserDetailsServerProfileDto>();
        }
    }
}
