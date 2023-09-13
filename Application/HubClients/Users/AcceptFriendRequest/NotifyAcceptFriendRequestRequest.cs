using MediatR;

namespace Application.HubClients.Users.AcceptFriendRequest
{
    public class NotifyAcceptFriendRequestRequest : IRequest
    {
        public int UserId { get; set; }
    }
}