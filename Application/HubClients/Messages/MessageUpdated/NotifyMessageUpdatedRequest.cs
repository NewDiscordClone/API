using MediatR;

namespace Sparkle.Application.HubClients.Messages.MessageUpdated
{
    public record NotifyMessageUpdatedRequest : IRequest
    {
        public string MessageId { get; init; }
    }
}