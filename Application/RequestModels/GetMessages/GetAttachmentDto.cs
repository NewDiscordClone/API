using Application.Models;

namespace Application.RequestModels.GetMessages
{
    public class GetAttachmentDto
    {
        public int Id { get; init; }
        public AttachmentType Type { get; init; }
        public string Path { get; init; }
        public bool IsSpoiler { get; init; }
    }
}