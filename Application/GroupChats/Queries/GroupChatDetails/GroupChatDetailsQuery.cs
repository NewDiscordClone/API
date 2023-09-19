using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.GroupChats.Queries.GroupChatDetails
{
    public record GroupChatDetailsQuery : IRequest<GroupChat>
    {
        /// <summary>
        /// Id of the Group Chat
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
    }
}