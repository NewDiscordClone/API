using MediatR;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Queries.GetServerDetails
{
    public record GetServerDetailsRequest : IRequest<ServerDetailsDto>
    {
        [Required]
        public string ServerId { get; init; }
    }
}
