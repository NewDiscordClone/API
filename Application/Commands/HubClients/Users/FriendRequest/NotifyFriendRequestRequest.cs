using MediatR;

namespace Application.Commands.HubClients.Users.FriendRequest
{
    public class NotifyFriendRequestRequest : IRequest
    {
        public Guid UserId { get; set; }
    }
}