using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Channels.RenameChannel
{
    public class RenameChannelRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public string NewTitle { get; init; }
    }
}