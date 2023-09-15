using MediatR;

namespace Sparkle.Application.Servers.Queries.GetServers
{
    public record GetServersQuery()
        : IRequest<List<GetServerLookupDto>>;
}