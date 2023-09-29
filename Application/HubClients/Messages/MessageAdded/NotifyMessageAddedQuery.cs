using MediatR;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.HubClients.Messages.MessageAdded
{
    public record NotifyMessageAddedQuery : IRequest
    {
        public MessageDto MessageDto { get; init; }
    }
}