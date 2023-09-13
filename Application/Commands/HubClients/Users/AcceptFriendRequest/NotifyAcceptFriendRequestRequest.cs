using MediatR;

namespace Application.Commands.HubClients.Users.AcceptFriendRequest
{
    public class NotifyAcceptFriendRequestRequest : IRequest
    {
        public Guid UserId { get; set; }
    }
}