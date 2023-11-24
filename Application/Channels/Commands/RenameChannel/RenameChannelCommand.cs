using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Channels.Commands.RenameChannel
{
    public record RenameChannelCommand : IRequest<Channel>
    {
        /// <summary>
        /// Id of the channel to be renamed
        /// </summary>
        public string ChatId { get; init; }

        /// <summary>
        /// New name of the channel
        /// </summary>
        public string NewTitle { get; init; }
    }
}