using MediatR;

namespace Sparkle.Application.HubClients.Servers.ServerUpdated
{
    public record NotifyServerUpdatedRequest : IRequest
    {
        public string ServerId { get; init; }
    }
}