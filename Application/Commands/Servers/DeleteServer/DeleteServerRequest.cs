using MediatR;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Commands.Servers.DeleteServer
{
    public record DeleteServerRequest : IRequest
    {
        [Required]
        public string ServerId { get; init; }
    }
}
