using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Connections.Disconnect
{
    public record DeleteUserConnectionCommand : IRequest<User?>
    {
        public string ConnectionId { get; init; }
    }
}