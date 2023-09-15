using MediatR;
using Sparkle.Application.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Servers.Commands.UpdateServer
{
    public record UpdateServerCommand : IRequest, IServerRequest
    {
        /// <summary>
        /// Id of the server to update
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }

        /// <summary>
        /// Server's name (Optional)
        /// </summary>
        [StringLength(32, MinimumLength = 1)]
        [DefaultValue("Server 1")]
        public string? Title { get; init; }

        /// <summary>
        /// Server's image url (Optional)
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
    }
}
