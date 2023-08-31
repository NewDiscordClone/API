using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddMessage
{
    public record AddMessageRequest : IRequest<Message>
    {
        [MaxLength(2000)]
        [DefaultValue("MessageText")]
        public string Text { get; init; }
        
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
        public List<Attachment>? Attachments { get; init; }
    }
}
