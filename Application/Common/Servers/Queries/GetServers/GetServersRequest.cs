using MediatR;

namespace Application.Common.Servers.Queries.GetServers
{
    public record GetServersRequest : IRequest<List<GetServerLookupDto>>
    {
    }
}