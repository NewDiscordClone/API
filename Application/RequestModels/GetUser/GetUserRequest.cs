using MediatR;

namespace Application.RequestModels.GetUser
{
    public class GetUserRequest : IRequest<GetUserDto>
    {
        public int UserId { get; init; }
    }
}