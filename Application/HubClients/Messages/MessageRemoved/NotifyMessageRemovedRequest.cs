using MediatR;

namespace Application.HubClients.Messages.MessageRemoved
{
    public record NotifyMessageRemovedRequest : IRequest
    {
        public string MessageId { get; init; }
        public string ChatId { get; init; }
    }
}