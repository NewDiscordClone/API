using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatRemoved
{
    public record NotifyPrivateChatRemovedQuery : IRequest
    {
        public string ChatId { get; init; }
        public Guid UserId { get; init; }
    }
}