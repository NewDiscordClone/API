using MediatR;

namespace Sparkle.Application.HubClients.Messages.MessageAdded
{
    public record NotifyMessageAddedQuery : IRequest
    {
        public string MessageId { get; init; }
    }
}