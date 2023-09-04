using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatUpdated
{
    public record NotifyPrivateChatUpdatedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}