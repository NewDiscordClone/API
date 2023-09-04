using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.Messages.UnpinMessage
{
    public record UnpinMessageRequest : IRequest<Message>
    {
        /// <summary>
        /// Id of message that need to be unpinned
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}