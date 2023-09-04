using MediatR;

namespace Application.Commands.HubClients.Connection.Disconnect
{
    public record DisconnectRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}