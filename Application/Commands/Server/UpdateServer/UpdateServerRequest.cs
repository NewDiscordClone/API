using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Server.UpdateServer
{
    public record UpdateServerRequest : IRequest
    {
        [Required]
        public int ServerId { get; init; }
        public string? Title { get; init; }
        public string? Image { get; init; }
    }
}
