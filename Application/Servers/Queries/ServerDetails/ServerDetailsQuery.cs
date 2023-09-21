using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Servers.Queries.ServerDetails
{
    public record ServerDetailsQuery : IRequest<ServerDetailsDto>
    {
        /// <summary>
        /// Id of the server
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
