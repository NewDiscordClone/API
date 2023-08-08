using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServer
{
    public class GetServerLookUpDto : IMapWith<Server>
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string? Image { get; init; }
        public List<GetServerChannelLookUpDto> Channels { get; init; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, GetServerLookUpDto>();
        }
    }
}