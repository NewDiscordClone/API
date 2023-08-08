using Application.Interfaces;
using Application.Models;
using Application.RequestModels.GetMessages;
using AutoMapper;

namespace Application.RequestModels.GetPrivateChats
{
    public record GetPrivateChatLookUpDto : IMapWith<PrivateChat>
    {
        public int Id { get; init; }
        public string? Image { get; init; }
        public string? Title { get; init; }
        public List<GetPrivateChatUserLookUpDto> Users { get; init; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrivateChat, GetPrivateChatLookUpDto>()
                // .ForMember(dto => dto.Users,
                // opt => opt.MapFrom(chat => chat.Users))
                ;
        }
    }
}