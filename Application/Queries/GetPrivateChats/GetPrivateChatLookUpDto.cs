using Application.Interfaces;
using Application.Models;
using Application.Queries.GetMessages;
using AutoMapper;
using MongoDB.Bson;

namespace Application.Queries.GetPrivateChats
{
    public record GetPrivateChatLookUpDto : IMapWith<PrivateChat>
    {
        public ObjectId Id { get; init; }
        public string? Image { get; init; }
        public string? Title { get; init; }
        public List<UserLookUp> Users { get; init; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrivateChat, GetPrivateChatLookUpDto>()
                // .ForMember(dto => dto.Users,
                // opt => opt.MapFrom(chat => chat.Users))
                ;
        }
    }
}