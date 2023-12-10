using MediatR;
using Sparkle.Domain.LookUps;

namespace Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles
{
    public record ServerProfilesQuery : IRequest<List<ServerProfileLookup>>
    {
        public string ServerId { get; init; }
    }
}
