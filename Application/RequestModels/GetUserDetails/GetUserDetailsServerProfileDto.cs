using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.RequestModels.GetUser
{
    public class GetUserDetailsServerProfileDto: IMapWith<ServerProfile>
    {
        public int Id { get; set; }
        public string? DisplayName { get; set; }
        
        public virtual List<GetUserDetailsRolesDto> Roles { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ServerProfile, GetUserDetailsServerProfileDto>();
        }
    }
}