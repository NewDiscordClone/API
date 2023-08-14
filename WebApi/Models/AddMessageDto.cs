using Application.Models;

namespace WebApi.Models
{
    public record AddMessageDto
    {
        public string Message { get; init; }
        public int ChatId { get; init; }
        public List<Attachment>? Attachments { get; init; }
    }
}
