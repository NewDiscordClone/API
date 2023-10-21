using MediatR;

namespace Sparkle.Application.HubClients.Users
{
    public class NotifyFriendRequestRequest : IRequest
    {
        public Guid UserId { get; set; }
    }
}