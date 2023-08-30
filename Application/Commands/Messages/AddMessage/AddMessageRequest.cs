using Application.Interfaces;
using Application.Models;

namespace Application.Commands.Messages.AddMessage
{
    public record AddMessageRequest : IServerRequest<Message>
    {
        public string ServerId { get; init; }
        public string Text { get; init; }
        public string ChatId { get; init; }
        public List<Attachment>? Attachments { get; init; }
    }
}
