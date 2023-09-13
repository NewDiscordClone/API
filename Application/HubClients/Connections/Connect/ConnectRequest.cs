using MediatR;

namespace Application.HubClients.Connections.Connect
{
    public record ConnectRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}