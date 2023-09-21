using MediatR;
using System.ComponentModel;

namespace Sparkle.Application.Channels.Commands.CreateChannel
{
    public class CreateChannelCommand : IRequest<string>
    {
        /// <summary>
        /// Name of the channel 
        /// </summary>
        [DefaultValue("Channel")]
        public string Title { get; init; }

        /// <summary>
        /// Id of the server where chat will be created
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ServerId { get; init; }
    }
}