using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public record NotifyPrivateChatSavedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}