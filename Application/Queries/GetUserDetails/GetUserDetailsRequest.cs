using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetUser
{
    public class GetUserDetailsRequest : IRequest<GetUserDetailsDto>
    {
        public int UserId { get; init; }
        public string? ServerId { get; init; }
    }
}