using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatCreated
{
    public record NotifyPrivateChatCreatedRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}