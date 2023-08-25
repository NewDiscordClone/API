using MediatR;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Commands.Servers.UpdateServer
{
    public record UpdateServerRequest : IRequest
    {
        [Required]
        public ObjectId ServerId { get; init; }
        public string? Title { get; init; }
        public string? Image { get; init; }
    }
}
