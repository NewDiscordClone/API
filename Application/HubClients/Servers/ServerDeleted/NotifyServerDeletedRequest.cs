using Application.Models;
using MediatR;

namespace Application.HubClients.Servers.ServerDeleted
{
    public record NotifyServerDeletedRequest : IRequest
    {
        public Server Server { get; init; }
    }
}