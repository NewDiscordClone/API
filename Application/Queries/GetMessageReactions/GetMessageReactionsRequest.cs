using MediatR;

namespace Application.Queries.GetMessageReactions
{
    public class GetMessageReactionsRequest : IRequest<List<UserReactionDto>>
    {
        public int MessageId { get; init; }
    }
}