using MediatR;

namespace Application.Queries.GetUser
{
    public class GetUserDetailsRequest : IRequest<GetUserDetailsDto>
    {
        public int UserId { get; init; }
        public int? ServerId { get; init; }
    }
}