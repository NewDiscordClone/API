using Application.Common.Interfaces;
using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Servers.Commands.DeleteServer
{
    public record DeleteServerRequest : IRequest<Server>, IServerRequest
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
