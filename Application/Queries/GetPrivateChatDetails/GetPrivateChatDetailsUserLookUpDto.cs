using Application.Interfaces;
using Application.Models;
using Application.Queries.GetMessages;
using AutoMapper;

namespace Application.Queries.GetPrivateChatDetails
{
    public record GetPrivateChatDetailsUserLookUpDto : IMapWith<User>
    {
        public int Id { get; init; }
        public string DisplayName { get; init; }
        public string AvatarPath { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetPrivateChatDetailsUserLookUpDto>();
        }
    }
}