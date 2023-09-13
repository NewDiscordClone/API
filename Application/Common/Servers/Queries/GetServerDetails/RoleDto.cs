using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using System.ComponentModel;

namespace Application.Common.Servers.Queries.GetServerDetails
{
    public record RoleDto : IMapWith<Role>
    {
        /// <summary>
        /// The unique identifier for the role.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        [DefaultValue("Admin")]
        public string Name { get; init; }

        /// <summary>
        /// The color associated with the role in hexadecimal format (e.g., "#FF0000" for red).
        /// </summary>
        [DefaultValue("#FF0000")]
        public string Color { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDto>();
        }
    }
}