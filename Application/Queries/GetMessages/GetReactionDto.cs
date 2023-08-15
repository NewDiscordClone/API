using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetMessages
{
    public record GetReactionDto : IMapWith<Reaction>
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public string UserId { get; init; }
        public string Emoji { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Reaction, GetReactionDto>()
                .ForMember(dto => dto.UserName,
                opt => opt.MapFrom(react => react.User.DisplayName));
        }
    }
}