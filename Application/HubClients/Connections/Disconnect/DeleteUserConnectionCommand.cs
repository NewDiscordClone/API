using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Connections.Disconnect
{
    public record DeleteUserConnectionCommand : IRequest<User?>
    {
        public string ConnectionId { get; init; }
    }
}