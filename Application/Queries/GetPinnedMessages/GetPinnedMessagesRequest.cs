using MediatR;

namespace Application.Queries.GetPinnedMessages
{
    public class GetPinnedMessagesRequest : IRequest<List<GetPinnedMessageLookUpDto>>
    {
        public int ChatId { get; init; }
    }
}