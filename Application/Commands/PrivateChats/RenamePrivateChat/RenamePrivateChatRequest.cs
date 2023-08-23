using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.RenamePrivateChat
{
    public class RenamePrivateChatRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public string NewTitle { get; init; }
    }
}