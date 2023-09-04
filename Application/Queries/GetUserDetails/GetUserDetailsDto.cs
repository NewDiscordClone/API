using Application.Interfaces;
using Application.Models;
using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Queries.GetUser
{
    public record GetUserDetailsDto : IMapWith<User>
    {
        /// <summary>
        /// Unique user identifier.
        /// </summary>
        [DefaultValue(1)]
        public int Id { get; init; }

        /// <summary>
        /// Non-unique display name shown to other users.
        /// </summary>
        [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string DisplayName { get; init; }

        /// <summary>
        /// Unique username for the user.
        /// </summary>
        [DefaultValue("username")]
        public string Username { get; init; }

        /// <summary>
        /// Avatar URL of the user.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string AvatarPath { get; init; }

        /// <summary>
        /// User's current status.
        /// </summary>
        [DefaultValue(UserStatus.Online)]
        public UserStatus Status { get; init; } = UserStatus.Online;

        /// <summary>
        /// User's current text status message.
        /// </summary>
        [DefaultValue("I'm Good")]
        public string? TextStatus { get; init; }

        public GetUserDetailsServerProfileDto? ServerProfile { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserDetailsDto>();
        }

    }
}