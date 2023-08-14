using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetUser
{
    public class GetUserDetailsRolesDto : IMapWith<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, GetUserDetailsRolesDto>();
        }
    }
}