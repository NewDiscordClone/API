using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Channels.RemoveChannel
{
    public class RemoveChannelRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
    }
}