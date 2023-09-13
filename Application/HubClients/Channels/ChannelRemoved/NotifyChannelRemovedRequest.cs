using MediatR;

namespace Application.HubClients.Channels.ChannelRemoved
{
    public record NotifyChannelRemovedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}