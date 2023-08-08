using MediatR;

namespace Application.RequestModels.GetMessages
{
    public record GetMessagesRequest : IRequest<List<GetMessageLookUpDto>>
    {
        public int ChatId { get; init; }
        public int MessagesCount { get; init; }
    }
}