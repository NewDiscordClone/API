using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;

namespace Application.Commands.Messages.EditMessage
{
    public class EditMessageRequest : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }
        
        [Required]
        [MaxLength(2000)]
        [MinLength(1)]
        [DefaultValue("NewTextString")]
        public string NewText { get; init; }
    }
}