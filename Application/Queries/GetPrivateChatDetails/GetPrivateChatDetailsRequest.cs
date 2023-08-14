using MediatR;

namespace Application.Queries.GetPrivateChatDetails
{
    public class GetPrivateChatDetailsRequest : IRequest<GetPrivateChatDetailsDto>
    {
        public int ChatId { get; init; }
    }
}