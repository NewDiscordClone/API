using MediatR;

namespace Sparkle.Application.HubClients.Channels.ChannelUpdated
{
    public record NotifyChannelUpdatedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}