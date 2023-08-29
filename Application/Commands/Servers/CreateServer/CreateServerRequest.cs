using System.ComponentModel;
using MediatR;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Commands.Servers.CreateServer
{
    public record CreateServerRequest : IRequest<string>
    {
        [Required, MaxLength(100)]
        public string Title { get; init; }

        [DataType(DataType.ImageUrl)]
        [DefaultValue("/")]
        public string? Image { get; init; }
    }
}
