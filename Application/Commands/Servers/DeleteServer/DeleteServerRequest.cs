using Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Servers.DeleteServer
{
    public record DeleteServerRequest : IServerRequest
    {
        [Required]
        public int ServerId { get; init; }
    }
}
