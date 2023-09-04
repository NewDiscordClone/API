using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Queries.GetGroupChatDetails
{
    public record GetGroupChatDetailsRequest : IRequest<GroupChat>
    {
        /// <summary>
        /// Id of the Group Chat
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
    }
}