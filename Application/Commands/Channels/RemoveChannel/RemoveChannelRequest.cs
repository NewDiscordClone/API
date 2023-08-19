using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.Channels.RemoveChannel
{
    public class RemoveChannelRequest : IRequest
    {
        public int ChatId { get; init; }
    }
}