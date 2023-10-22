using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Servers.Commands.CreateServer
{
    public record CreateServerCommand : IRequest<Server>
    {
        /// <summary>
        /// New Server's name
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// Server image url
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
        public ServerTemplates? Template { get; init; } = ServerTemplates.Default;
    }

    public enum ServerTemplates
    {
        Default,
        Gaming,
        Study,
        School,
        Friends
    }
}
