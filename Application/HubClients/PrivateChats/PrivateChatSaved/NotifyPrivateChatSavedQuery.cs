using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public record NotifyPrivateChatSavedQuery : IRequest
    {
        public Chat Chat { get; init; }
    }
}