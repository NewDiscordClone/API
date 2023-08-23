using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.RemoveMessage
{
    public class RemoveMessageRequest : IRequest<Chat>
    {
        public ObjectId MessageId { get; init; }
    }
}