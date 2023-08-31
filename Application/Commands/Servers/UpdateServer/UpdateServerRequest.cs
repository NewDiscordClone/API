using System.ComponentModel;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;

namespace Application.Commands.Servers.UpdateServer
{
    public record UpdateServerRequest : IRequest, IServerRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
        [DefaultValue("Server 1")]
        public string? Title { get; init; }
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
    }
}
