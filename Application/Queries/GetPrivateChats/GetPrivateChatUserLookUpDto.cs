using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetMessages
{
    public record GetPrivateChatUserLookUpDto : IMapWith<User>
    {
        public int Id { get; init; }
        public string DisplayName { get; init; }
        public string AvatarPath { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetMessageUserDto>();
        }
    }
}