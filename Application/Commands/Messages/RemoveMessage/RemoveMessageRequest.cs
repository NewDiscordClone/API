using Application.Models;
using MediatR;

namespace Application.Commands.Messages.RemoveMessage
{
    public class RemoveMessageRequest : IRequest<Chat>
    {
        public int MessageId { get; init; }
    }
}