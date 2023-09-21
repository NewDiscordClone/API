using MediatR;

namespace Sparkle.Application.HubClients.Channels.ChannelUpdated
{
    public record NotifyChannelUpdatedQuery : IRequest
    {
        public string ChannelId { get; init; }
    }
}