using MediatR;

namespace Sparkle.Application.HubClients.Connections.Disconect
{
    public record DisconnectRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}