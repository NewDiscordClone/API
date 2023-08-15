namespace Application.Queries.GetMessageReactions
{
    public class UserReactionDto
    {
        public GetReactionDto Reaction { get; init; }
        public GetUserLookUpDto User { get; init; }
    }
}