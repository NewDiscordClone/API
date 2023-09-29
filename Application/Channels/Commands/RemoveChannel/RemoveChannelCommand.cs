using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.Channels.Commands.RemoveChannel
{
    public class RemoveChannelCommand : IRequest<Channel>
    {
        /// <summary>
        /// Id of the channel to be removed 
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
    }
}