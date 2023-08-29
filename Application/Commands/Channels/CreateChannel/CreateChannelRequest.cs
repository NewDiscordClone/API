using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;

namespace Application.Commands.Channels.CreateChannel
{
    public class CreateChannelRequest : IRequest<Channel>
    {
        [MaxLength(255)] 
        public string Title { get; init; }

        public string ServerId { get; init; }
    }
}