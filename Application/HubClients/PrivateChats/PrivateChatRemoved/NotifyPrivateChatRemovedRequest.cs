using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatRemoved
{
    public record NotifyPrivateChatRemovedRequest : IRequest
    {
        public string ChatId { get; init; }
        public Guid UserId { get; init; }
    }
}