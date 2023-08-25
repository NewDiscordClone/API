using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MongoDB.Bson;

namespace Application.Queries.GetServer
{
    public record GetServerLookupDto : IMapWith<Server>
    {
        public ObjectId Id { get; init; }
        public string Title { get; init; }
        public string? Image { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Server, GetServerLookupDto>();
        }
    }
}