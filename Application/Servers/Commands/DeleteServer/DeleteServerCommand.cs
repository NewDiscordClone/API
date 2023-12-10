using MediatR;
using Sparkle.Domain;
using System.ComponentModel;

namespace Sparkle.Application.Servers.Commands.DeleteServer
{
    public record DeleteServerCommand : IRequest<(Server Server, IEnumerable<Guid> UserIds)>
    {
        /// <summary>
        /// Id of the server to delete
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
