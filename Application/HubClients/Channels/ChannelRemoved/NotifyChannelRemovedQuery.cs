using MediatR;

namespace Sparkle.Application.HubClients.Channels.ChannelRemoved
{
    public record NotifyChannelRemovedQuery : IRequest
    {
        public string ChannelId { get; init; }
    }
}