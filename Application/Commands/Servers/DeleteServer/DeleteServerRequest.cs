using Application.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Servers.DeleteServer
{
    public record DeleteServerRequest : IRequest, IServerRequest
    {
        [Required]
        public string ServerId { get; init; }
    }
}
