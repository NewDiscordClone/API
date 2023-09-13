using MediatR;

namespace Sparkle.Application.HubClients.Users.FriendRequest
{
    public class NotifyFriendRequestRequest : IRequest
    {
        public Guid UserId { get; set; }
    }
}