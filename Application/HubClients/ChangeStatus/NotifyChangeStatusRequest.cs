using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.ChangeStatus
{
    public record NotifyChangeStatusRequest : IRequest
    {
        public UserStatus Status { get; init; }
    }
}