using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatSaved
{
    public record NotifyPrivateChatSavedQuery : IRequest
    {
        public Chat Chat { get; init; }
    }
}