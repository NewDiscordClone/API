using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Common.Servers.Queries.GetServerDetails
{
    public record GetServerDetailsRequest : IRequest<ServerDetailsDto>
    {
        /// <summary>
        /// Id of the server
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
