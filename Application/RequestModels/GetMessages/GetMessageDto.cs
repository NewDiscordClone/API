#nullable enable
using Application.RequestModels.GetServer;
using Application.RequestModels.GetUser;

namespace Application.RequestModels.GetMessages
{
    public class GetMessageDto
    {
        public int Id { get; init; }
        public GetUserDto User { get; init; }
        public string Text { get; init; }
        public DateTime SendTime { get; init; }
        public List<GetAttachmentDto> Attachments { get; init; }
        public List<GetReactionDto> Reactions { get; init; }
        public GetServerProfileDto? ServerProfileDto { get; init; }
    }
}