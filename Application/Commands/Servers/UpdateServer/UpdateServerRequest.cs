using Application.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Servers.UpdateServer
{
    public record UpdateServerRequest : IServerRequest
    {
        [Required]
        [DefaultValue("64edc7e5053fc67916bd8f58")]
        public string ServerId { get; init; }
        [DefaultValue("Server 1")]
        public string? Title { get; init; }
        [DefaultValue(null)]
        public string? Image { get; init; }
    }
}
