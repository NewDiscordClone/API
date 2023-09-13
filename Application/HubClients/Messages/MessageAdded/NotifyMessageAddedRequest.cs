using MediatR;

namespace Application.HubClients.Messages.MessageAdded
{
    public record NotifyMessageAddedRequest : IRequest
    {
        public string MessageId { get; init; }
    }
}