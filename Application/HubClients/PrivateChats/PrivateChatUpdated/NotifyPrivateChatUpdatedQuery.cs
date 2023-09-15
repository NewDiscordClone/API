using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatUpdated
{
    public record NotifyPrivateChatUpdatedQuery : IRequest
    {
        public string ChatId { get; init; }
    }
}