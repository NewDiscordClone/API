using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Channels.ChannelRemoved
{
    public record NotifyChannelRemovedQuery : IRequest
    {
        public Channel Channel { get; init; }
    }
}