using MediatR;

namespace Application.Commands.HubClients.Channels.ChannelCreated
{
    public record NotifyChannelCreatedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}