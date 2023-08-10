using MediatR;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequest : IRequest<GetPrivateChatDetailsDto>
    {
        public int UserId { get; init; }
        public int ChatId { get; init; }
    }
}