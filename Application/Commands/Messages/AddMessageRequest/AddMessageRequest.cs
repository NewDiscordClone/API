using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddMessageRequest
{
    public record AddMessageRequest : IRequest<Message>
    {
        public string Text { get; init; }
        public int ChatId { get; init; }
        public List<AddMessageAttachmentDto>? Attachments { get; init; }
    }
}
