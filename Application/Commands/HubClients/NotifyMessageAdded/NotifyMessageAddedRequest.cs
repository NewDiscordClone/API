using Application.Models;
using MediatR;

namespace Application.Commands.NotifyClients.NotifyChatMembers
{
    public record NotifyMessageAddedRequest : IRequest
    {
        public int MessageId { get; init; } 
    }
}