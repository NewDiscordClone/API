using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesRequest : IRequest<List<Message>>
    {
        public ObjectId ChatId { get; init; }
    }
}