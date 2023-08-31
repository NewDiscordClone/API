using MediatR;

namespace Application.Commands.HubClients.Connection.OnDisconnected
{
    public record OnDisconnectedRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}