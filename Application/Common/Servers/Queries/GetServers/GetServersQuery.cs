using MediatR;

namespace Sparkle.Application.Common.Servers.Queries.GetServers
{
    public record GetServersQuery()
        : IRequest<List<GetServerLookupDto>>;
}