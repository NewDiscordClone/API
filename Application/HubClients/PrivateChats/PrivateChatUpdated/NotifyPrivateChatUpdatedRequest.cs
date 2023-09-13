using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatUpdated
{
    public record NotifyPrivateChatUpdatedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}