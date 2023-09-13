using MediatR;

namespace Application.HubClients.Channels.ChannelUpdated
{
    public record NotifyChannelUpdatedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}