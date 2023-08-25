using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetMedia
{
    public class GetMediaRequest : IRequest<Media>
    {
        public ObjectId Id { get; set; }
        //public string Extension { get; set; }
    }
}