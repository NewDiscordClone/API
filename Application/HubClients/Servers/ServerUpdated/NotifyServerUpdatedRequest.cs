using MediatR;

namespace Application.HubClients.Servers.ServerUpdated
{
    public record NotifyServerUpdatedRequest : IRequest
    {
        public string ServerId { get; init; }
    }
}