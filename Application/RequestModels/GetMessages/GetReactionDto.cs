using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.RequestModels.GetMessages
{
    public record GetReactionDto : IMapWith<Reaction>
    {
        public int Id { get; init; }
        public GetMessageUserDto User { get; init; }
        public string Emoji { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Reaction, GetReactionDto>();
        }
    }
}