using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.RequestModels.GetServer
{
    public class GetServerLookupDto : IMapWith<Server>
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string? Image { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, GetServerLookupDto>();
        }
    }
}