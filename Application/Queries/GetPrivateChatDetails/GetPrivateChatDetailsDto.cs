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
        public GetPrivateChatUserLookUpDto Owner { get; init; }
        public List<GetPrivateChatUserLookUpDto> Users { get; init; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PrivateChat, GetPrivateChatDetailsDto>()
                // .ForMember(dto => dto.Users,
                // opt => opt.MapFrom(chat => chat.Users))
                ;
        }
    }
}