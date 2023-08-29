using Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Servers.UpdateServer
{
    public record UpdateServerRequest : IServerRequest
    {
        [Required]
        public int ServerId { get; init; }
        public string? Title { get; init; }
        public string? Image { get; init; }
    }
}
