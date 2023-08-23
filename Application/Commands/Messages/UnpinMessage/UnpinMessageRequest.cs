using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.UnpinMessage
{
    public class UnpinMessageRequest : IRequest<Message>
    {
        public ObjectId MessageId { get; init; }
    }
}