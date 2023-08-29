using Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Application.Commands.Servers.DeleteServer
{
    public record DeleteServerRequest : IServerRequest
    {
        [Required]
        public string ServerId { get; init; }
    }
}
