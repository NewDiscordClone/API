using Application.Models;
using Application.RequestModels.GetMessages;

namespace Application.RequestModels.GetServer
{
    public class GetChannelDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public List<GetMessageDto> Messages { get; init; }
    }
}