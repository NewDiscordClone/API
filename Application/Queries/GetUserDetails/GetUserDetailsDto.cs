using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetUser
{
    public record GetUserDetailsDto : IMapWith<User>
    {
        public Guid Id { get; init; }
        [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string DisplayName { get; init; }
        [DefaultValue("username")]
        public string Username { get; init; }
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string AvatarPath { get; init; }
        [DefaultValue(UserStatus.Online)]
        public UserStatus Status { get; init; } = UserStatus.Online;
        [DefaultValue("I'm Good")]
        public string? TextStatus { get; init; }
        public GetUserDetailsServerProfileDto? ServerProfile { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserDetailsDto>();
        }
        
    }
}