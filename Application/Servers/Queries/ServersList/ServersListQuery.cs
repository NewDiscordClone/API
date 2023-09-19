using MediatR;

namespace Sparkle.Application.Servers.Queries.ServersList
{
    public record ServersListQuery()
        : IRequest<List<ServerLookUpDto>>;
}