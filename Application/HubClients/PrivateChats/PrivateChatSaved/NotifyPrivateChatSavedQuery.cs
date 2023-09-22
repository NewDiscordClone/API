using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public record NotifyPrivateChatSavedQuery : IRequest
    {
        public string ChatId { get; init; }
    }
}