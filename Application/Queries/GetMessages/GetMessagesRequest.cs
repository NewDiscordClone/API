using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models.LookUps;
using MediatR;

namespace Application.Queries.GetMessages
{
    public record GetMessagesRequest : IRequest<List<MessageDto>>
    {
        /// <summary>
        /// The unique identifier of the chat from which to retrieve messages.
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        /// <summary>
        /// The number of messages that have already been loaded and should be skipped.
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int MessagesCount { get; init; }
    }
}
