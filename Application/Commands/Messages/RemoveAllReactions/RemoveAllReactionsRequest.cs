using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.RemoveAllReactions
{
    public class RemoveAllReactionsRequest : IRequest<Chat>
    {
        public string MessageId { get; init; }
    }
}