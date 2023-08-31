using MediatR;

namespace Application.Commands.HubClients.Channels.ChannelRemoved
{
    public record NotifyChannelRemovedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}