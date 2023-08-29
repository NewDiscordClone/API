using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetMessages
{
    public record GetMessagesRequest : IRequest<List<Message>>
    {
        public string ChatId { get; init; }
        public int MessagesCount { get; init; }
    }
}