using MediatR;

namespace Application.Queries.GetMessages
{
    public record GetMessagesRequest : IRequest<List<GetMessageLookUpDto>>
    {
        public int ChatId { get; init; }
        public int MessagesCount { get; init; }
    }
}