using MediatR;
using Sparkle.Domain.LookUps;

namespace Sparkle.Application.HubClients.Messages.MessageAdded
{
    public record NotifyMessageAddedQuery : IRequest
    {
        public MessageDto MessageDto { get; init; }
    }
}