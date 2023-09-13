using MediatR;

namespace Application.HubClients.Messages.MessageUpdated
{
    public record NotifyMessageUpdatedRequest : IRequest
    {
        public string MessageId { get; init; }
    }
}