using MediatR;

namespace Application.Commands.HubClients.Messages.MessageAdded
{
    public record NotifyMessageAddedRequest : IRequest
    {
        public string MessageId { get; init; }
    }
}