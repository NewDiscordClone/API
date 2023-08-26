using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Channels.CreateChannel
{
    public class CreateChannelRequest : IRequest<Channel>
    {
        [MaxLength(255)] 
        public string Title { get; init; }

        public ObjectId ServerId { get; init; }
    }
}