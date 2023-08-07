using MediatR;

namespace Application.RequestModels.GetMessages
{
    public record GetMessagesRequest : IRequest<List<GetMessageDto>>
    {
        public int ChatId { get; init; }
        public int MessagesCount { get; init; }
    }
}