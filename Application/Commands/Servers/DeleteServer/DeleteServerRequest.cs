using System.ComponentModel;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Application.Models;

namespace Application.Commands.Servers.DeleteServer
{
    public record DeleteServerRequest : IRequest<Server>, IServerRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}
