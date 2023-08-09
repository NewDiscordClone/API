using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetServerDetails
{
    public record ChannelLookupDto : IMapWith<Channel>
    {
        public int Id { get; init; }
        public string Title { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Channel, ChannelLookupDto>();
        }
    }
}