using MediatR;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public record ConnectRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}