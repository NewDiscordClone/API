using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Servers.DeleteServer
{
    public record DeleteServerRequest : IRequest
    {
        [Required]
        public int ServerId { get; init; }
    }
}
