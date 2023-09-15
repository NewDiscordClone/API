using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Servers.Commands.DeleteServer
{
    public record DeleteServerCommand : IRequest<Server>, IServerRequest
    {
        /// <summary>
        /// Id of the server to delete
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
