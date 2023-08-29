using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.RemoveMessage
{
    public class RemoveMessageRequest : IRequest<Chat>
    {
        public string MessageId { get; init; }
    }
}