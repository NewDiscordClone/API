using MediatR;

namespace Application.HubClients.PrivateChats.PrivateChatUpdated
{
    public record NotifyPrivateChatUpdatedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}