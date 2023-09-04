using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Queries.GetPinnedMessages
{
    public record GetPinnedMessagesRequest : IRequest<List<Message>>
    {
        /// <summary>
        /// The unique identifier of the chat from which to retrieve pinned messages.
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
    }
}
