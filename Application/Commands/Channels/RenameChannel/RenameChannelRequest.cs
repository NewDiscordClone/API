using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.Channels.RenameChannel
{
    public class RenameChannelRequest : IRequest
    {
        public int ChatId { get; init; }
        public string NewTitle { get; init; }
    }
}