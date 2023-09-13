using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Messages.Commands.RemoveMessage
{
    public record RemoveMessageRequest : IRequest<Chat>
    {
        /// <summary>
        /// Id of the message to remove
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}