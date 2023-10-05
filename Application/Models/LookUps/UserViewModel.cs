using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models.LookUps
{
    /// <summary>
    /// User dto for collections
    /// </summary>
    public class UserViewModel : IMapWith<User>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Non-unique user name
        /// </summary>
        public string? DisplayName { get; set; }

        public string? UserName { get; set; }
        /// <summary>
        /// Avatar url
        /// </summary>
        [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }
        public string? TextStatus { get; set; }

        public UserStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserViewModel>();
        }
    }
}