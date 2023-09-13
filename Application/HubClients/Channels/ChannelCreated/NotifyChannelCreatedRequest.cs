using MediatR;

namespace Sparkle.Application.HubClients.Channels.ChannelCreated
{
    public record NotifyChannelCreatedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}