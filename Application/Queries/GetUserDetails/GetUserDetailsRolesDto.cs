using System.ComponentModel;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetUser
{
    public class GetUserDetailsRolesDto : IMapWith<Role>
    {
        public Guid Id { get; set; }
        [DefaultValue("Admin")]
        public string Name { get; set; }
        [DefaultValue("#FF0000")]
        public string Color { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, GetUserDetailsRolesDto>();
        }
    }
}