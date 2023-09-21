using MediatR;

namespace Sparkle.Application.HubClients.Servers.ServerUpdated
{
    public record NotifyServerUpdatedQuery : IRequest
    {
        public string ServerId { get; init; }
    }
}