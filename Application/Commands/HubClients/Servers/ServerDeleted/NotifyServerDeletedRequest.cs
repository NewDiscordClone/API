using Application.Models;
using MediatR;

namespace Application.Commands.HubClients.Servers.ServerDeleted
{
    public record NotifyServerDeletedRequest : IRequest
    {
        public Server Server { get; init; }
    }
}