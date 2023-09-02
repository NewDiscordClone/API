using MediatR;

namespace Application.Commands.HubClients.Users.AcceptFriendRequest
{
    public class NotifyAcceptFriendRequestRequest : IRequest
    {
        public int UserId { get; set; }
    }
}