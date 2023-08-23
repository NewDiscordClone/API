using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.PinMessage
{
    public class PinMessageRequest : IRequest<Message>
    {
        public ObjectId MessageId { get; init; }
    }
}