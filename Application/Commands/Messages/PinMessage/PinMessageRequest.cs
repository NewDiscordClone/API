using Application.Models;
using MediatR;

namespace Application.Commands.Messages.PinMessage
{
    public class PinMessageRequest : IRequest<Message>
    {
        public int MessageId { get; init; }
    }
}