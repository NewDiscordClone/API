using MediatR;

namespace Application.Commands.HubClients.Users.FriendRequest
{
    public class NotifyFriendRequestRequest : IRequest
    {
        public int UserId { get; set; }
    }
}