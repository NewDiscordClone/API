using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.AddMessage
{
    public record AddMessageRequest : IRequest<Message>
    {
        public string Text { get; init; }
        public string ChatId { get; init; }
        public List<Attachment>? Attachments { get; init; }
    }
}
