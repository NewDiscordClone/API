using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.HubClients.Servers.ServerDeleted
{
    public record NotifyServerDeletedQuery : IRequest
    {
        public Server Server { get; init; }
    }
}