using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetPrivateChatDetails
{
    public record GetPrivateChatDetailsDto : IMapWith<PrivateChat>
    {
        public int Id { get; init; }
        public string? Image { get; init; }
        public string? Title { get; init; }
        public GetPrivateChatDetailsUserLookUpDto Owner { get; init; }
        public List<GetPrivateChatDetailsUserLookUpDto> Users { get; init; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrivateChat, GetPrivateChatDetailsDto>()
                // .ForMember(dto => dto.Users,
                // opt => opt.MapFrom(chat => chat.Users))
                ;
        }
    }
}