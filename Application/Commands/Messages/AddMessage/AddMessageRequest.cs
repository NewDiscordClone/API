using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddMessage
{
    public record AddMessageRequest : IRequest<Message>, IServerRequest
    {
        public string ServerId { get; init; }
        public string Text { get; init; }
        public string ChatId { get; init; }
        public List<Attachment>? Attachments { get; init; }
    }
}
