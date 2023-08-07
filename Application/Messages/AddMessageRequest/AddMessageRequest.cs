using Application.Models;
using MediatR;

namespace Application.Messages.AddMessageRequest
{
    public record AddMessageRequest : IRequest<Message>
    {
        public string Text { get; init; }
        public int UserId { get; set; }
        public int ChatId { get; init; }
    }
}
