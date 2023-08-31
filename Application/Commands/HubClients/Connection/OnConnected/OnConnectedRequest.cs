using MediatR;

namespace Application.Commands.HubClients.Connection.OnConnected
{
    public record OnConnectedRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}