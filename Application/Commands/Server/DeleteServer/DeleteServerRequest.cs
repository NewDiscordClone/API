using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Server.DeleteServer
{
    public record DeleteServerRequest : IRequest
    {
        [Required]
        public int Id { get; init; }
    }
}
