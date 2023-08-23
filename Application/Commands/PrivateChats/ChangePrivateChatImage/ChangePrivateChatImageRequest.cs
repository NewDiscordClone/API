using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public string NewImage { get; init; }
    }
}