using MediatR;

namespace Sparkle.Application.Common.Servers.Queries.GetServers
{
    public record GetServersRequest : IRequest<List<GetServerLookupDto>>
    {
    }
}