using MediatR;

namespace Sparkle.Application.Channels.Commands.RenameChannel
{
    public record RenameChannelCommand : IRequest
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