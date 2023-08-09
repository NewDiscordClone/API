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

        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int UserId { get; init; }
    }
}
