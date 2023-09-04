using MediatR;

namespace Application.Commands.HubClients.Channels.UpdateChannel
{
    public record NotifyChannelUpdatedRequest : IRequest
    {
        public string ChannelId { get; init; }
    }
}