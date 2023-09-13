using MediatR;

namespace Application.HubClients.Users.FriendRequest
{
    public class NotifyFriendRequestRequest : IRequest
    {
        public int UserId { get; set; }
    }
}