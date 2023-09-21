using MediatR;

namespace Sparkle.Application.Users.Queries.GetUserIdFromUserName
{
    public record GetUserIdFromUserNameQuery : IRequest<Guid>
    {
        public string UserName { get; init; }
    }
}