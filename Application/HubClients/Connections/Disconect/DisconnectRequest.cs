using MediatR;

namespace Application.HubClients.Connections.Disconect
{
    public record DisconnectRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}