using MediatR;

namespace Application.Commands.HubClients.Servers.ServerUpdated
{
    public record NotifyServerUpdatedRequest : IRequest
    {
        public string ServerId { get; init; }
    }
}