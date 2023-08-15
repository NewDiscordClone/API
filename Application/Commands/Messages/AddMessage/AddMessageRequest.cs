using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddMessage
{
    public record AddMessageRequest : IRequest<int>
    {
        public string Text { get; init; }
        public int ChatId { get; init; }
        public List<AddMessageAttachmentDto>? Attachments { get; init; }
    }
}
