using MediatR;

namespace Sparkle.Application.HubClients.PrivateChats.PrivateChatCreated
{
    public record NotifyPrivateChatCreatedQuery : IRequest
    {
        public string ChatId { get; init; }
    }
}