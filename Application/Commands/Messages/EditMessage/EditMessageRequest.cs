using Application.Models;
using MediatR;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequest : IRequest<Message>
    {
        public int MessageId { get; init; }
        public string NewText { get; init; }
    }
}