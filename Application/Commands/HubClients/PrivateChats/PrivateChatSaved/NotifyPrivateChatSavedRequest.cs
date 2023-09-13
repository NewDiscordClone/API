using MediatR;

namespace Application.Commands.HubClients.PrivateChats.PrivateChatSaved
{
    public record NotifyPrivateChatSavedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}