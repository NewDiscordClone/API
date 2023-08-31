using System.ComponentModel;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetUser
{
    public class GetUserDetailsServerProfileDto: IMapWith<ServerProfile>
    {
        [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string? DisplayName { get; set; }
        
        public virtual List<GetUserDetailsRolesDto> Roles { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ServerProfile, GetUserDetailsServerProfileDto>()
                // .ForMember(sp => sp.Roles, 
                //     opt => opt
                //         .MapFrom(sp => sp.Roles))
                ;
        }
    }
}