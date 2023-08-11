using Application.Models;
using MediatR;

namespace Application.Commands.Messages.PinMessage
{
    public class PinMessageRequest : IRequest
    {
        public int MessageId { get; init; }
    }
}