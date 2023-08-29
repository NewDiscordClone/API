using System.ComponentModel;
using MediatR;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Commands.Servers.UpdateServer
{
    public record UpdateServerRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
        public string? Title { get; init; }
        [DefaultValue("/")]
        [DataType(DataType.ImageUrl)]
        public string? Image { get; init; }
    }
}
