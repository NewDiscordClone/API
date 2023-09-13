using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using System.ComponentModel;

namespace Application.Users.Queries.GetUserDetails
{
    public record GetUserDetailsRolesDto : IMapWith<Role>
    {
        /// <summary>
        /// The unique identifier for the role.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        [DefaultValue("Admin")]
        public string Name { get; set; }

        /// <summary>
        /// The color associated with the role in hexadecimal format (e.g., "#FF0000" for red).
        /// </summary>
        [DefaultValue("#FF0000")]
        public string Color { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, GetUserDetailsRolesDto>();
        }
    }
}