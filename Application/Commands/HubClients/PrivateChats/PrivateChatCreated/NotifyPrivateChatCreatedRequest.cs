using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatCreated
{
    public record NotifyPrivateChatCreatedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}