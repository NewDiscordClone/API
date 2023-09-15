using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Messages.Commands.RemoveAllReactions
{
    public record RemoveAllReactionsCommand : IRequest
    {
        /// <summary>
        /// Id of the message for which all reactions should be removed
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
    }
}