using Application.Models;
using MediatR;

namespace Application.Commands.Messages.UnpinMessage
{
    public class UnpinMessageRequest : IRequest<Message>
    {
        public int MessageId { get; init; }
    }
}