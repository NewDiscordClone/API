using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public record AddUserConnectionCommand : IRequest<User?>
    {
        public string ConnectionId { get; init; }
    }
}