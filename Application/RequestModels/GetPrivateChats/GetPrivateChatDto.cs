#nullable enable
using Application.RequestModels.GetMessages;
using Application.RequestModels.GetUser;

namespace Application.RequestModels.GetPrivateChats
{
    public class GetPrivateChatDto
    {
        public int Id { get; init; }
        public string? Image { get; init; }
        public string? Title { get; init; }
    
        public List<GetUserDto> Users { get; init; } = new();
        public List<GetMessageDto> Messages { get; init; } = new();
    }
}