using MediatR;

namespace Sparkle.Application.HubClients.Messages.MessageUpdated
{
    public record NotifyMessageUpdatedQuery : IRequest
    {
        public string MessageId { get; init; }
    }
}