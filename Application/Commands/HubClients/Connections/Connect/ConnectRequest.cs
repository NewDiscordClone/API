using MediatR;

namespace Application.Commands.HubClients.Connection.Connect
{
    public record ConnectRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}