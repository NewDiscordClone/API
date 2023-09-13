using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Servers.ServerDeleted
{
    public record NotifyServerDeletedRequest : IRequest
    {
        public Server Server { get; init; }
    }
}