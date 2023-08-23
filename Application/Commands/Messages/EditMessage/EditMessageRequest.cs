using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequest : IRequest<Message>
    {
        public ObjectId MessageId { get; init; }
        public string NewText { get; init; }
    }
}