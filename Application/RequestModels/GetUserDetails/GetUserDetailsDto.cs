using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.RequestModels.GetUser
{
    public record GetUserDetailsDto : IMapWith<User>
    {
        public int Id { get; init; }
        public string DisplayName { get; init; }
        public string Username { get; init; }
        public string AvatarPath { get; init; }
        public UserStatus Status { get; init; } = UserStatus.Online;
        public string? TextStatus { get; init; }
        public ServerProfile? Profile { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserDetailsDto>();
        }
    }
}