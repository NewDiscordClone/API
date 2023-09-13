using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Servers.Commands.CreateServer
{
    public record CreateServerRequest : IRequest<string>
    {
        /// <summary>
        /// New Server's name
        /// </summary>
        [Required, MaxLength(100)]
        public string Title { get; init; }

        /// <summary>
        /// Server image url
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
    }
}
