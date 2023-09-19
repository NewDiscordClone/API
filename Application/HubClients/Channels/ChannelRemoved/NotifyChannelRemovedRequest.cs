using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Channels.ChannelRemoved
{
    public record NotifyChannelRemovedRequest : IRequest
    {
        public Channel Channel { get; init; }
    }
}