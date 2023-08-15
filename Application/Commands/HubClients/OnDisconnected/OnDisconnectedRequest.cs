using MediatR;

namespace Application.Commands.NotifyClients.OnDisconnected
{
    public record OnDisconnectedRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}