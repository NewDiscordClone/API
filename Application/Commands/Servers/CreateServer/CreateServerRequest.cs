using MediatR;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Commands.Servers.CreateServer
{
    public record CreateServerRequest : IRequest<ObjectId>
    {
        [Required, MaxLength(255)]
        public string Title { get; init; }

        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }
    }
}
