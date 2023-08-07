using Application.RequestModels.GetUser;

namespace Application.RequestModels.GetMessages
{
    public class GetReactionDto
    {
        public int Id { get; init; }
        public GetUserDto User { get; init; }
        public string Emoji { get; init; }
    }
}