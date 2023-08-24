using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddMessage
{
    public record AddMessageRequest : IRequest<Message>
    {
        public string Text { get; init; }
        public int ChatId { get; init; }
        public int ServerId { get; init; }
        public List<AddMessageAttachmentDto>? Attachments { get; init; }
    }
}
