using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Server.CreateServer
{
    public record CreateServerRequest : IRequest<int>
    {
        [Required, MaxLength(255)]
        public string Title { get; init; }

        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }
    }
}
