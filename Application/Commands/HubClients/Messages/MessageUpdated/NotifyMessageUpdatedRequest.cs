using MediatR;

namespace Application.Commands.HubClients.Messages.MessageUpdated
{
    public record NotifyMessageUpdatedRequest : IRequest
    {
        public string MessageId { get; init; }
    }
}