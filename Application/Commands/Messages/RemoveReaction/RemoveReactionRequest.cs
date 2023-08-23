using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.RemoveReaction
{
    public class RemoveReactionRequest : IRequest<Message>
    {
        public ObjectId MessageId { get; init; }
        public int ReactionIndex { get; init; }
    }
}