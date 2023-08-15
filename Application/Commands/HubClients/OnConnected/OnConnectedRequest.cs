using MediatR;

namespace Application.Commands.NotifyClients.OnConnected
{
    public record OnConnectedRequest : IRequest
    {
        public string ConnectionId { get; init; }
    }
}