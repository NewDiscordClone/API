using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequest : IRequest<Message>
    {
        public string MessageId { get; init; }
        public string NewText { get; init; }
    }
}