using MediatR;

namespace Application.HubClients.PrivateChats.PrivateChatCreated
{
    public record NotifyPrivateChatCreatedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}