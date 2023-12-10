using MediatR;
using Sparkle.Domain;

namespace Sparkle.Application.HubClients.Connections.Connect
{
    public record AddUserConnectionCommand : IRequest<User?>
    {
        public string ConnectionId { get; init; }
    }
}